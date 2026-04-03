using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public interface IMultiPool<Key> : IPoolCommon
    {
        bool HasKey(Key p_Key);
        bool RemovePool(Key p_Key);
        void Preload(Key p_Key, int p_LoadCount);
        void RetrieveAll(Key p_Key);
        void ClearPool(Key p_Key);
    }

    public interface IPoolCluster<Key> : IMultiPool<Key>
    {
        IPool this[Key p_Key] { get; }
        bool TryGetPool(Key p_Key, out IPool o_Pool);
        bool TryAddPool(Key p_Key, IPool p_Pool, int p_PreloadCount);
    }
    
    public interface IObjectPoolCluster<Key, CreateParams, ActivateParams> : IPoolCluster<Key>
        where CreateParams : IObjectCreateParams
        where ActivateParams : IObjectActivateParams
    {
        Return Pop<Return>(Key p_Key, ActivateParams p_ActivateParams) where Return : ObjectPoolContent<Return, CreateParams, ActivateParams>, new();
    }
    
    public interface IPrefabPoolCluster<Key, CreateParams, ActivateParams> : IPoolCluster<Key>
        where CreateParams : IPrefabCreateParams<CreateParams>
        where ActivateParams : IPrefabActivateParams
    {
        Return Pop<Return>(Key p_Key, ActivateParams p_ActivateParams) where Return : PrefabPoolContent<Return, CreateParams, ActivateParams>;
    }

    public interface IPoolSocket<Key, Content, in CreateParams, in ActivateParams> : IMultiPool<Key>
        where Content : IContent
        where CreateParams : ICreateParams
        where ActivateParams : IActivateParams
    {
        Content Pop(Key p_Key, ActivateParams p_ActivateParams);
        Return Pop<Return>(Key p_Key, ActivateParams p_ActivateParams) where Return : Content;
        void GetActivateList(ref List<Content> r_Result);
    }
    
    public interface IObjectPoolSocket<Key, Content, CreateParams, ActivateParams> : IPoolSocket<Key, Content, CreateParams, ActivateParams>
        where Content : ObjectPoolContent<Content, CreateParams, ActivateParams>, new()
        where CreateParams : IObjectCreateParams
        where ActivateParams : IObjectActivateParams
    {
        ObjectPool<Content, CreateParams, ActivateParams> this[Key p_Key] { get; }
        bool TryGetPool(Key p_Key, out ObjectPool<Content, CreateParams, ActivateParams> o_Pool);
        bool TryAddPool(Key p_Key, CreateParams p_CreateParams, int p_PreloadCount);
    }
    
    public interface IPrefabPoolSocket<Key, Content, CreateParams, ActivateParams> : IPoolSocket<Key, Content, CreateParams, ActivateParams>
        where Content : PrefabPoolContent<Content, CreateParams, ActivateParams>
        where CreateParams : IPrefabCreateParams<CreateParams>
        where ActivateParams : IPrefabActivateParams
    {
        PrefabPool<Content, CreateParams, ActivateParams> this[Key p_Key] { get; }
        bool TryGetPool(Key p_Key, out PrefabPool<Content, CreateParams, ActivateParams> o_Pool);
        bool TryAddPool(Transform p_Affine, Key p_Key, CreateParams p_CreateParams, int p_PreloadCount);
        UniTask<bool> TryAddPool(Transform p_Affine, Key p_Key, CreateParams p_CreateParams, int p_PreloadCount, CancellationToken p_CancellationToken);
    }
}