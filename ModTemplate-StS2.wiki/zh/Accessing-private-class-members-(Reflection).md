# **（本页面正在建设中）**
***
私有类成员通常是有意被设为私有的，但在模组开发生态系统中，有时确实需要有能力访问和修改它们。本页概述了从你的代码中访问私有类成员（例如私有字段、方法）的 3 种主要方式。

## AccessTools
### FieldRef

### Field/Method

### 关于后备字段的说明


## Publicizer
Publicizer 是一个允许你在编译时访问私有字段的库。你需要在 `csproj` 文件中进行定义。
关于该库的更多信息请参见[此处](https://github.com/krafs/Publicizer#usage)。

> [!NOTE]
> 如果在添加新的 publicized 字段后出现红色波浪线提示，请清理解决方案然后重新构建。（在 Rider 中：点击顶部菜单的 `Build`，然后选择 `Clean Solution`）

### Publicize 目标
要 publicize 一个具体的目标，你需要按 `<Publicize Include="dllName:Namespace.ClassMember"/>` 的格式定义类成员。完整示例见下文。

完整示例：
```xml
    <!--  清理解决方案时清除缓存  -->
    <PropertyGroup>
        <PublicizerClearCacheOnClean>true</PublicizerClearCacheOnClean>
    </PropertyGroup>

    <!--  以 dllName:Namespace.ClassMember 格式定义的 publicized 类成员  -->
    <ItemGroup>
        <Publicize Include="sts2:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand._combatState"/>
    </ItemGroup>

    <!--  Nuget 包引用  -->
    <ItemGroup>
        <PackageReference Include="Krafs.Publicizer" Version="2.3.0" PrivateAssets="All">
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        </PackageReference>
    </ItemGroup>
```
### Publicize all


## Traverse
