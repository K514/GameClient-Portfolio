using System.Collections.Generic;
using k514.Mono.Common;

namespace k514
{
    public interface IPool : IPoolCommon
    {
        /// <summary>
        /// 풀에 p_CheckNumber보다 많아지지 않을 만큼 p_Number개의 Content를 생성하고 바로 풀링하는 메서드
        /// </summary>
        void Preload(int p_LoadCount);

        /// <summary>
        /// 현재 풀에서 관리하는 Content 갯수를 리턴하는 메서드
        /// </summary>
        int GetContentCount();

        /// <summary>
        /// 현재 활성화된 Content의 갯수를 리턴하는 메서드
        /// </summary>
        int GetActivateContentCount();
    }

    public interface IPool<Content> : IPool where Content : IContent
    {
        /// <summary>
        /// 지정한 Content를 풀링하는 메서드
        /// </summary>
        void Retrieve(Content p_Content);

        /// <summary>
        /// 현재 활성화중인 Content 리스트 반복자를 리턴하는 메서드
        /// </summary>
        List<Content> GetActiveEnumerator();
        
        /// <summary>
        /// 지정한 Content의 인덱스를 리턴하는 메서드
        /// </summary>
        int GetIndex(Content p_Content);

        /// <summary>
        /// 지정한 Content를 해당 풀로부터 제외하는 메서드
        /// </summary>
        void DisconnectContent(Content p_Content);
    }
    
    public interface IPool<Content, in ActivateParams> : IPool<Content> where Content : IContent
    {
        /// <summary>
        /// 풀에서 Content 하나를 리턴하는 메서드
        /// </summary>
        Content Pop(ActivateParams p_ActivateParams);
    }
    
    public interface IPool<Content, out CreateParams, in ActivateParams> : IPool<Content,  ActivateParams> where Content : IContent
    {
        /// <summary>
        /// 풀 키를 리턴하는 메서드
        /// </summary>
        CreateParams GetCreateParams();
    }

    public interface IContent : _IDisposable
    {
        /// <summary>
        /// 현재 Content 상태
        /// </summary>
        PoolTool.ContentState ContentState { get; }
        
        /// <summary>
        /// 현재 Content가 활성화 상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsContentActive { get; }

        /// <summary>
        /// 현재 Content가 풀링 상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsContentPooled { get; }
        
        /// <summary>
        /// 해당 Content를 풀링하는 메서드
        /// </summary>
        void Pooling();
        
        /// <summary>
        /// 해당 Content가 풀링된 횟수
        /// </summary>
        int GetPooledCount();

        /// <summary>
        /// 해당 Content를 Pool로부터 독립시키는 메서드
        /// </summary>
        void DisconnectPool();
    }

    public interface IObjectContent<CreateParams> : IContent where CreateParams : IObjectCreateParams
    {
        /// <summary>
        /// 생성 파라미터를 리턴하는 메서드
        /// </summary>
        CreateParams GetCreateParams();

        /// <summary>
        /// 해당 Content가 풀을 거치지 않고 생성된 경우, 생성 콜백을 수동으로 호출해주는 메서드
        /// </summary>
        void CheckAwake(CreateParams p_CreateParams = default);
    }
    
    public interface IPrefabContent<CreateParams> : IContent, IAffine where CreateParams : IPrefabCreateParams<CreateParams>
    {
        /// <summary>
        /// 생성 파라미터를 리턴하는 메서드
        /// </summary>
        CreateParams GetCreateParams();

        /// <summary>
        /// 해당 Content가 풀을 거치지 않고 생성된 경우, 생성 콜백을 수동으로 호출해주는 메서드
        /// </summary>
        void CheckAwake(CreateParams p_CreateParams = default);
        
        /// <summary>
        /// 게임 오브젝트의 레이어를 변경하는 메서드
        /// </summary>
        void TurnLayerTo(GameConst.GameLayerType p_LayerType);
    }
}