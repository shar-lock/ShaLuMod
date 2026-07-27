// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereMinigame
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;

public class CrystalSphereMinigame
{
  private const int _defaultWidth = 11;
  private const int _defaultHeight = 11;
  private int _divinationCount;
  private readonly TaskCompletionSource _completionSource = new TaskCompletionSource();
  private readonly Player _owner;
  public CrystalSphereCell[,] cells;
  private readonly List<CrystalSphereItem> _items = new List<CrystalSphereItem>();
  private readonly List<CrystalSphereItem> _revealed = new List<CrystalSphereItem>();

  public Rng Rng { get; private set; }

  public int DivinationCount
  {
    get => this._divinationCount;
    set
    {
      this._divinationCount = value;
      Action divinationCountChanged = this.DivinationCountChanged;
      if (divinationCountChanged == null)
        return;
      divinationCountChanged();
    }
  }

  public Vector2I GridSize => new Vector2I(this.cells.GetLength(0), this.cells.GetLength(1));

  public bool IsFinished => this.DivinationCount == 0;

  public bool PlacedAllItems { get; private set; }

  public CrystalSphereMinigame.CrystalSphereToolType CrystalSphereTool { get; private set; }

  public event Action? DivinationCountChanged;

  public event Action? Finished;

  public IReadOnlyList<CrystalSphereItem> Items => (IReadOnlyList<CrystalSphereItem>) this._items;

  private CrystalSphereCell? HoveredCell { get; set; }

  private List<CrystalSphereCell> HighlightedCells { get; set; } = new List<CrystalSphereCell>();

  public CrystalSphereMinigame(Player owner, Rng rng, int divinationCount)
  {
    this._owner = owner;
    this.Rng = rng;
    this.cells = new CrystalSphereCell[11, 11];
    for (int x = 0; x < 11; ++x)
    {
      for (int y = 0; y < 11; ++y)
        this.cells[x, y] = new CrystalSphereCell(x, y);
    }
    int capacity = 4;
    List<Vector2I> vector2IList1 = new List<Vector2I>(capacity);
    CollectionsMarshal.SetCount<Vector2I>(vector2IList1, capacity);
    Span<Vector2I> span = CollectionsMarshal.AsSpan<Vector2I>(vector2IList1);
    int num1 = 0;
    span[num1] = new Vector2I(0, 0);
    int num2 = num1 + 1;
    span[num2] = new Vector2I(this.cells.GetLength(0) - 1, 0);
    int num3 = num2 + 1;
    span[num3] = new Vector2I(this.cells.GetLength(0) - 1, this.cells.GetLength(0) - 1);
    int num4 = num3 + 1;
    span[num4] = new Vector2I(0, this.cells.GetLength(0) - 1);
    List<Vector2I> vector2IList2 = vector2IList1;
    for (int index = 0; index < 2; ++index)
      vector2IList2 = vector2IList2.Concat<Vector2I>(vector2IList2.SelectMany<Vector2I, Vector2I>((Func<Vector2I, IEnumerable<Vector2I>>) (c => (IEnumerable<Vector2I>) this.GetHorizontalCells(c.X, c.Y)))).Concat<Vector2I>(vector2IList2.SelectMany<Vector2I, Vector2I>((Func<Vector2I, IEnumerable<Vector2I>>) (c => (IEnumerable<Vector2I>) this.GetVerticalCells(c.X, c.Y)))).ToList<Vector2I>();
    foreach (Vector2I vector2I in vector2IList2)
      TaskHelper.RunSafely(this.ClearCell(vector2I.X, vector2I.Y));
    int num5 = 0;
    do
    {
      this.PlacedAllItems = this.PopulateItems();
      ++num5;
    }
    while (!this.PlacedAllItems && num5 < 10);
    this.DivinationCount = divinationCount;
    this.CrystalSphereTool = CrystalSphereMinigame.CrystalSphereToolType.Big;
  }

  public void ForceMinigameEnd()
  {
    this._revealed.Clear();
    if (this._completionSource.Task.IsCompleted)
      return;
    this._completionSource.SetCanceled();
  }

  public async Task PlayMinigame()
  {
    if (!LocalContext.IsMe(this._owner))
      return;
    NCrystalSphereScreen.ShowScreen(this);
    await this._completionSource.Task;
    await this.CompleteMinigame();
  }

  private bool PopulateItems()
  {
    bool flag1 = true;
    CrystalSphereItem crystalSphereItem1 = (CrystalSphereItem) new CrystalSphereRelic();
    bool flag2 = flag1 && crystalSphereItem1.PlaceItem(this);
    this._items.Add(crystalSphereItem1);
    for (int index = 0; index < 2; ++index)
    {
      CrystalSphereItem crystalSphereItem2 = (CrystalSphereItem) new CrystalSpherePotion(PotionRarity.Common);
      flag2 = flag2 && crystalSphereItem2.PlaceItem(this);
      this._items.Add(crystalSphereItem2);
    }
    CrystalSphereItem crystalSphereItem3 = (CrystalSphereItem) new CrystalSpherePotion(PotionRarity.Rare);
    bool flag3 = flag2 && crystalSphereItem3.PlaceItem(this);
    this._items.Add(crystalSphereItem3);
    CrystalSphereItem crystalSphereItem4 = (CrystalSphereItem) new CrystalSphereCardReward(CardRarity.Common, this._owner);
    bool flag4 = flag3 && crystalSphereItem4.PlaceItem(this);
    this._items.Add(crystalSphereItem4);
    CrystalSphereItem crystalSphereItem5 = (CrystalSphereItem) new CrystalSphereCardReward(CardRarity.Uncommon, this._owner);
    bool flag5 = flag4 && crystalSphereItem5.PlaceItem(this);
    this._items.Add(crystalSphereItem5);
    CrystalSphereItem crystalSphereItem6 = (CrystalSphereItem) new CrystalSphereCardReward(CardRarity.Rare, this._owner);
    bool flag6 = flag5 && crystalSphereItem6.PlaceItem(this);
    this._items.Add(crystalSphereItem6);
    CrystalSphereItem crystalSphereItem7 = (CrystalSphereItem) new CrystalSphereCurse();
    bool flag7 = flag6 && crystalSphereItem7.PlaceItem(this);
    this._items.Add(crystalSphereItem7);
    for (int index = 0; index < 5; ++index)
    {
      CrystalSphereItem crystalSphereItem8 = (CrystalSphereItem) new CrystalSphereGold(false);
      flag7 = flag7 && crystalSphereItem8.PlaceItem(this);
      this._items.Add(crystalSphereItem8);
    }
    for (int index = 0; index < 2; ++index)
    {
      CrystalSphereItem crystalSphereItem9 = (CrystalSphereItem) new CrystalSphereGold(true);
      flag7 = flag7 && crystalSphereItem9.PlaceItem(this);
      this._items.Add(crystalSphereItem9);
    }
    foreach (CrystalSphereItem crystalSphereItem10 in this._items)
      crystalSphereItem10.Revealed += new Action<CrystalSphereItem>(this.OnItemRevealed);
    return flag7;
  }

  private void OnItemRevealed(CrystalSphereItem item) => this._revealed.Add(item);

  public void SetHoveredCell(CrystalSphereCell cell)
  {
    if (this.HoveredCell != null)
      this.UnsetHoveredCell();
    this.HoveredCell = cell;
    this.HoveredCell.IsHovered = true;
    if (this.CrystalSphereTool == CrystalSphereMinigame.CrystalSphereToolType.Big)
    {
      this.HighlightedCells = this.GetAdjacentCells(cell.X, cell.Y).Select<Vector2I, CrystalSphereCell>((Func<Vector2I, CrystalSphereCell>) (c => this.cells[c.X, c.Y])).ToList<CrystalSphereCell>();
    }
    else
    {
      int capacity = 1;
      List<CrystalSphereCell> crystalSphereCellList = new List<CrystalSphereCell>(capacity);
      CollectionsMarshal.SetCount<CrystalSphereCell>(crystalSphereCellList, capacity);
      CollectionsMarshal.AsSpan<CrystalSphereCell>(crystalSphereCellList)[0] = this.cells[cell.X, cell.Y];
      this.HighlightedCells = crystalSphereCellList;
    }
    foreach (CrystalSphereCell highlightedCell in this.HighlightedCells)
      highlightedCell.IsHighlighted = true;
  }

  public void UnsetHoveredCell()
  {
    foreach (CrystalSphereCell highlightedCell in this.HighlightedCells)
    {
      highlightedCell.IsHighlighted = false;
      highlightedCell.IsHovered = false;
    }
    this.HighlightedCells = new List<CrystalSphereCell>();
    this.HoveredCell = (CrystalSphereCell) null;
  }

  public void SetTool(CrystalSphereMinigame.CrystalSphereToolType tool)
  {
    this.CrystalSphereTool = tool;
    if (this.HoveredCell == null)
      return;
    this.SetHoveredCell(this.HoveredCell);
  }

  public async Task CellClicked(CrystalSphereCell clickedCell)
  {
    this.DivinationCount--;
    if (this.CrystalSphereTool == CrystalSphereMinigame.CrystalSphereToolType.Big)
    {
      foreach (Vector2I adjacentCell in this.GetAdjacentCells(clickedCell.X, clickedCell.Y))
        await this.ClearCell(adjacentCell.X, adjacentCell.Y);
    }
    else
      await this.ClearCell(clickedCell.X, clickedCell.Y);
    if (this.DivinationCount != 0)
      return;
    this._completionSource.SetResult();
  }

  private async Task ClearCell(int x, int y)
  {
    if (x < 0 || x >= this.GridSize.X)
      throw new ArgumentException($"[{x},{y}] is not a valid cell on this grid");
    if (y < 0 || y >= this.GridSize.Y)
      throw new ArgumentException($"[{x},{y}] is not a valid cell on this grid");
    if (!this.cells[x, y].IsHidden)
      return;
    this.cells[x, y].IsHidden = false;
    if (this.cells[x, y].Item == null)
      return;
    CrystalSphereItem crystalSphereItem = this.cells[x, y].Item;
    if (!this.AreAllOccupiedCellsClear(crystalSphereItem))
      return;
    await crystalSphereItem.RevealItem(this._owner);
  }

  private bool AreAllOccupiedCellsClear(CrystalSphereItem item)
  {
    for (int index1 = 0; index1 < item.Size.X; ++index1)
    {
      for (int index2 = 0; index2 < item.Size.Y; ++index2)
      {
        if (this.cells[item.Position.X + index1, item.Position.Y + index2].IsHidden)
          return false;
      }
    }
    return true;
  }

  private async Task CompleteMinigame()
  {
    await Cmd.Wait(0.75f);
    await RunManager.Instance.OneOffSynchronizer.DoLocalCrystalSphereRewards(this._owner, this.Rng, this._revealed);
    Action finished = this.Finished;
    if (finished == null)
      return;
    finished();
  }

  private List<Vector2I> GetAdjacentCells(int x, int y)
  {
    // ISSUE: object of a compiler-generated type is created
    return this.GetHorizontalCells(x, y).Concat<Vector2I>((IEnumerable<Vector2I>) this.GetVerticalCells(x, y)).Concat<Vector2I>((IEnumerable<Vector2I>) this.GetDiagonalCells(x, y)).Concat<Vector2I>((IEnumerable<Vector2I>) new \u003C\u003Ez__ReadOnlySingleElementList<Vector2I>(new Vector2I(x, y))).ToList<Vector2I>();
  }

  private List<Vector2I> GetHorizontalCells(int x, int y)
  {
    List<Vector2I> horizontalCells = new List<Vector2I>();
    for (int index = -1; index <= 1; index += 2)
    {
      int num = x + index;
      if (num >= 0 && num < 11)
        horizontalCells.Add(new Vector2I(num, y));
    }
    return horizontalCells;
  }

  private List<Vector2I> GetVerticalCells(int x, int y)
  {
    List<Vector2I> verticalCells = new List<Vector2I>();
    for (int index = -1; index <= 1; index += 2)
    {
      int num = y + index;
      if (num >= 0 && num < 11)
        verticalCells.Add(new Vector2I(x, num));
    }
    return verticalCells;
  }

  private List<Vector2I> GetDiagonalCells(int x, int y)
  {
    List<Vector2I> diagonalCells = new List<Vector2I>();
    for (int index1 = -1; index1 <= 1; index1 += 2)
    {
      for (int index2 = -1; index2 <= 1; index2 += 2)
      {
        int num1 = x + index1;
        int num2 = y + index2;
        if (num1 >= 0 && num1 < 11 && num2 >= 0 && num2 < 11)
          diagonalCells.Add(new Vector2I(num1, num2));
      }
    }
    return diagonalCells;
  }

  public enum CrystalSphereToolType
  {
    None,
    Small,
    Big,
  }
}
