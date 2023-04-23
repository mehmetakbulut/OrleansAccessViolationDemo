Interacting with persistent state causes an access violation if the grain deactivates following a `DeactivateOnIdle()` call inside a timer which has called a grain method via [non-reentrancy workaround.](https://github.com/dotnet/orleans/issues/2574)

To reproduce, build and run project.

```
fail: Orleans.Grain[100516]
      Error calling grain's OnDeactivateAsync(...) method - Grain type = Demo.MyGrain Activation = [Activation: S127.0.0.1:11111:41218990/my/dab8a3d360654eb791414946dddade41@a208e821061c40f4b81c5dde14dc3ef1#GrainType=Demo.MyGrain,Demo Placement=RandomPlacement State=Deactivating]
      System.InvalidOperationException: Activation access violation. A non-activation thread attempted to access activation services.
         at Orleans.Runtime.GrainRuntime.<CheckRuntimeContext>g__ThrowMissingContext|20_0() in /_/src/Orleans.Runtime/Core/GrainRuntime.cs:line 93
         at Orleans.Runtime.GrainRuntime.CheckRuntimeContext(IGrainContext context) in /_/src/Orleans.Runtime/Core/GrainRuntime.cs:line 92
         at Orleans.Core.StateStorageBridge`1.set_State(TState value) in /_/src/Orleans.Runtime/Storage/StateStorageBridge.cs:line 35
         at Orleans.Runtime.PersistentStateFactory.PersistentStateBridge`1.set_State(TState value) in /_/src/Orleans.Runtime/Facet/Persistent/PersistentStateStorageFactory.cs:line 74
         at Demo.MyGrain.OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken) in OrleansAccessViolationDemo\OrleansAccessViolationDemo\Demo\MyGrain.cs:line 23
         at Orleans.Runtime.ActivationData.<FinishDeactivating>g__CallGrainDeactivate|135_0(CancellationToken ct) in /_/src/Orleans.Runtime/Catalog/ActivationData.cs:line 1456
```
