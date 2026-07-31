# -*- coding: utf-8 -*-
import re, subprocess
diff = subprocess.run(['git','diff','--ignore-all-space','doc/万敌Mod-卡牌设计.md'],
                      capture_output=True, text=True, encoding='utf-8').stdout
def norm(s):
    return re.sub(r'\s+','',s).replace('　','')
lines = diff.split('\n')
dels, adds = [], []
for l in lines:
    if l.startswith('-') and not l.startswith('---'):
        dels.append(l[1:])
    elif l.startswith('+') and not l.startswith('+++'):
        adds.append(l[1:])
def is_cardrow(s):
    return s.strip().startswith('|') and ('/' in s or '伤' in s or '血' in s or '纷争' in s or '生命' in s)
del_cards = [d for d in dels if is_cardrow(d)]
add_cards = [a for a in adds if is_cardrow(a)]
print("=== 卡牌行实质改动（旧→新）===")
for d in del_cards:
    nd = norm(d)
    if nd in {norm(a) for a in add_cards}:
        continue
    # 找同卡名（前缀）增行
    match = None
    for a in add_cards:
        if nd[:10] == norm(a)[:10]:
            match = a; break
    print(f"\n- 旧: {d.strip()}")
    if match:
        print(f"+ 新: {match.strip()}")
    else:
        print("+ 新: (无匹配/整行删除)")
