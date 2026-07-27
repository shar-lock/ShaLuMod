# **(Page under construction)**
***
Private class members are usually private for a reason, but in a modding ecosystem, it is sometimes necessary to have the ability to access and edit them. This page outlines the 3 key ways of accessing private class members (eg., private fields, methods) from your code.

## AccessTools
### FieldRef

### Field/Method

### Notes on backing fields


## Publicizer
Publicizer is a library that allows you to access private fields via at compile time. You define them within your `csproj` file.
For more information the library see [here](https://github.com/krafs/Publicizer#usage).

> [!NOTE]
> If you have having red linting lines after adding a new publicized field, clean your solution and then rebuild. (in Rider: click `Build` in the top menu, then `Clean Solution`)

### Publicize target
To publicize a specific target, you need to define the class member in the format `<Publicize Include="dllName:Namespace.ClassMember"/>`. See below for full example.

Complete example:
```xml
    <!--  Clear cache when cleaning the solution  -->
    <PropertyGroup>
        <PublicizerClearCacheOnClean>true</PublicizerClearCacheOnClean>
    </PropertyGroup>

    <!--  Publicized class members in the format: dllName:Namespace.ClassMember  -->
    <ItemGroup>
        <Publicize Include="sts2:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand._combatState"/>
    </ItemGroup>

    <!--  Nuget package reference  -->
    <ItemGroup>
        <PackageReference Include="Krafs.Publicizer" Version="2.3.0" PrivateAssets="All">
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        </PackageReference>
    </ItemGroup>
```
### Publicize all


## Traverse