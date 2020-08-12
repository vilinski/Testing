# UnitTest

> Helper Utility for testing purposes

## Benefits
- Automatic object initialization
- simple code readability
- simple code maintainability
- concentration only on the test case


## How to use

### Add Nuget 
ifm.Suite.BuildingBlock.UnitTest

### Use Attribut

Use `[Theory, AutoDataMoq]` in your test method to autogenarate Mocks and Objects.

#### Sample A
You can have your test class created by the framework (here e.g. `GroupedHostedService groupedHostedService`) and all its dependencies are created automatically

```csharp
[Theory, AutoDataMoq(5)]
public async Task StartAsync_WithDefaultModuleDescriptions_VerifyHostedServices_SuccessfullProcessing(
    GroupedHostedService groupedHostedService)
{
    await groupedHostedService.StartAsync(CancellationToken.None);

    Assert.True(groupedHostedService.HasCompletedSuccessfull());
}
```

#### Sample B
If you want to test a member of a class property (here e.g. `CommandQueue commandQueue`), you have to add this property before the test class itself and add the attribute `[Frozen]`
```csharp
[Theory, AutoDataMoq(5)]
public async Task StartAsync_WithDefaultModuleDescriptions_VerifyHostedServices_SuccessfullProcessing(
    [Frozen] CommandQueue commandQueue
    GroupedHostedService groupedHostedService)
{
    await groupedHostedService.StartAsync(CancellationToken.None);

    Assert.True(groupedHostedService.HasCompletedSuccessfull());
    Assert.True(commandQueue.ProcessMessages);
}
```

#### Sample C
If you want to indicate that the parameter value should not have properties auto populated, you have to add the attribute `[NoAutoProperties]`
```csharp
[Theory, AutoDataMoq(5)]
public async Task StartAsync_WithDefaultModuleDescriptions_VerifyHostedServices_SuccessfullProcessing(
    [Frozen][NoAutoProperties] CommandQueue commandQueue
    GroupedHostedService groupedHostedService)
{
    await groupedHostedService.StartAsync(CancellationToken.None);

    Assert.True(groupedHostedService.HasCompletedSuccessfull());
    Assert.True(commandQueue.ProcessMessages);
}
```

### Sample D
If you wand to `Setup` the behavior of a a Mock instance (here e.g. `Mock<IServiceManager> serviceManagerMock`), do it after injection
```csharp
//If you wand an array in your test case, you have the ability to configure the count of that array e.g 5
[Theory, AutoDataMoq(5)]
public async Task StartAsync_WithDefaultModuleDescriptions_VerifyHostedServices_SuccessfullProcessing(
    ModuleDescription[] moduleDescriptionDummy
    [Frozen]Mock<IServiceManager> serviceManagerMock
    [Frozen][NoAutoProperties] CommandQueue commandQueue
    GroupedHostedService groupedHostedService)
{
    //you can change the behavior of a method by using 'Setup' as below
    serviceManagerMock.Setup(p => p.GetAll()).Returns(moduleDescriptionDummy);

    await groupedHostedService.StartAsync(CancellationToken.None);

    Assert.True(groupedHostedService.HasCompletedSuccessfull());
    Assert.True(commandQueue.ProcessMessages);
    serviceManagerMock.Verify(p => p.GetAll(), Times.Once);
}
```

### Links
https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet

