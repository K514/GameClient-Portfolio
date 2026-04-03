using System;

namespace k514.Mono.Common
{
    public partial class GameEntityTool
    {
        #region <Enum>
        
        public enum GameEntityType
        {
            Gear,
            Unit,
            Vfx,
            Projectile,
            Projector,
            Beam,
            Audio,
            Video,
        }
        
        public enum EnemySelectType
        {
            /// <summary>
            /// 적을 찾지 않음. 기본값
            /// </summary>
            None,
            
            /// <summary>
            /// 가장 가까운 위치의 유닛을 선정함
            /// </summary>
            NearestPosition,
            
            /// <summary>
            /// 바라보는 방향과 가장 가까운 방향의 유닛을 선정함
            /// </summary>
            NearestAngle,
            
            /// <summary>
            /// 랜덤한 유닛을 선정함
            /// </summary>
            Random,
        }
        
        /// <summary>
        /// 오브젝트 재질 타입
        /// </summary>
        public enum MaterialType
        {
            /// <summary>
            /// 재질을 가지지 않는 오브젝트임
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 천 채질
            /// </summary>
            Fabric,
            
            /// <summary>
            /// 가죽 재질
            /// </summary>
            Leather,
            
            /// <summary>
            /// 뼈 재질
            /// </summary>
            Bone,
            
            /// <summary>
            /// 광물 재질
            /// </summary>
            Rock,
            
            /// <summary>
            /// 경금속 재질
            /// </summary>
            LightMetal,

            /// <summary>
            /// 중금속 재질
            /// </summary>
            HeavyMetal,

            /// <summary>
            /// 무형 재질
            /// </summary>
            Energy,
        }

        /// <summary>
        /// 오브젝트 원소 속성 타입
        /// </summary>
        [Flags]
        public enum ElementType
        {
            /// <summary>
            /// 무속성
            /// </summary>
            None = 0,
             
            /// <summary>
            /// 화속성
            /// </summary>
            Fire = 1 << 0,
            
            /// <summary>
            /// 수속성
            /// </summary>
            Water = 1 << 1,
            
            /// <summary>
            /// 풍속성
            /// </summary>
            Wind = 1 << 2,
            
            /// <summary>
            /// 지속성
            /// </summary>
            Ground = 1 << 3,
            
            /// <summary>
            /// 명속성
            /// </summary>
            Light = 1 << 4,
            
            /// <summary>
            /// 암속성
            /// </summary>
            Darkness = 1 << 5,
        }

        public static ElementType[] ElementTypeEnumerator;

        /// <summary>
        /// 개체 생성 타입
        /// </summary>
        public enum ActivateParamsAttributeType
        {
            None = 0,
            
            /// <summary>
            /// 활성화된 개체에 Player 속성 부여
            /// </summary>
            GivePlayer = 1 << 0,
            
            /// <summary>
            /// 활성화된 개체에 Boss 속성 부여
            /// </summary>
            GiveBoss = 1 << 1,
            
            /// <summary>
            /// 주인 개체가 사망/비활성화/회수 된 경우, 활성화된 개체도 파기됨
            /// </summary>
            GiveFollowFallenMaster = 1 << 2,
            
            /// <summary>
            /// 활성화된 개체에 PreserveCorpse 속성 부여
            /// </summary>
            GivePreserveCorpse = 1 << 3,
        }

        public static ActivateParamsAttributeType[] ActivateParamsAttributeTypeEnumerator;
        
        /// <summary>
        /// 개체 속성 타입
        /// </summary>
        [Flags]
        public enum GameEntityAttributeType
        {
            None = 0,

            /// <summary>
            /// 상호작용 갱신 속성
            /// </summary>
            InteractionUpdater = 1 << 1,
            
            /// <summary>
            /// 사망 이후에도 풀링되지 않는 속성
            /// </summary>
            PreserveCorpse= 1 << 5,
            
            /// <summary>
            /// 주인 개체 사망/풀링 시, 따라가는 속성
            /// </summary>
            FollowFallenMaster= 1 << 6,
     
            /// <summary>
            /// 최초 리롤을 무료로 수행한다.
            /// </summary>
            FirstRerollService = 1 << 10,
        }
        
        public static GameEntityAttributeType[] AttributeTypeEnumerator;

        /// <summary>
        /// 개체 수명 페이즈를 기술하는 열거형 상수
        /// </summary>
        public enum EntityLifeSpanPhase
        {
            None,
            LiveSpan,
            DeadSpan,
            LifeSpanTerminate,
        }

        /// <summary>
        /// 외력 연산 타입
        /// </summary>
        public enum AddForceType
        {
            /// <summary>
            /// 트리거에서 타겟 방향으로 힘이 작용한다.
            /// 타겟이 없다면 StrikerForward 로 동작한다.
            /// </summary>
            TargetDirection,
            
            /// <summary>
            /// 트리거 오브젝트가 바라보는 방향으로 힘이 작용한다.
            /// </summary>
            TriggerForward,
            
            /// <summary>
            /// 현재 모션의 시작 아핀 값을 기준으로 힘이 작용한다.
            /// </summary>
            CachedAnimationMotionAffine,
            
            /// <summary>
            /// 절대 벡터로 힘이 작용한다.
            /// </summary>
            WorldForce,
            
            /// <summary>
            /// 타격 판정의 시작점을 기준으로 힘이 작용한다.
            /// </summary>
            HitStartPosition,
            
            /// <summary>
            /// 타격 판정의 현재 기준점을 기준으로 힘이 작용한다.
            /// </summary>
            HitPivotPosition,
            
            /// <summary>
            /// 타격 판정이 이동하는 방향을 기준으로 힘이 작용한다.
            /// </summary>
            MultiHitMoveDirection,
        }

        /// <summary>
        /// 외력 적용 타입
        /// </summary>
        [Flags]
        public enum ForceControlType
        {
            /// <summary>
            /// 특별한 추가 처리 없이 외력이 적용됨
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 외력 시작점으로부터 일정 거리를 벗어나지 않도록 외력이 적용됨
            /// </summary>
            BoundDistance = 1 << 0,
            
            /// <summary>
            /// 외력 처리중 조우한 오브젝트를 끌고감
            /// </summary>
            DrawObject = 1 << 1,
                        
            /// <summary>
            /// 외력 처리중 조우한 오브젝트에게 피해를 줌
            /// </summary>
            HitObject = 1 << 2,
        }
        
        public static ForceControlType[] ObjectAddForceProcessTypeEnumerator;

        /// <summary>
        /// 오브젝트 환경음 타입
        /// </summary>
        [Flags]
        public enum EnvironmentSoundType
        {
            /// <summary>
            /// 해당 오브젝트는 소리를 내지 않음
            /// </summary>
            None = 0,

            /// <summary>
            /// 해당 오브젝트는 이동시 소리를 냄
            /// </summary>
            FootStep = 1 << 0,
        }
        
        public static EnvironmentSoundType[] ObjectEnvironmentSoundTypeEnumerator;

        /// <summary>
        /// 개체 상태 타입
        /// </summary>
        [Flags]
        public enum EntityStateType : long
        {
            /// <summary>
            /// 정상 상태
            /// </summary>
            None = 0L,
       
            /// <summary>
            /// 특정 이벤트를 수행 중인 상태
            /// </summary>
            DRIVE_EVENT = 1L << 0,
            
            /// <summary>
            /// 특정 명령을 수행 중인 상태
            /// </summary>
            DRIVE_ORDER = 1L << 1,
            
            /// <summary>
            /// 이동 액션을 수행 중인 상태
            /// </summary>
            DRIVE_MOVE = 1L << 2,

            /// <summary>
            /// 대시 액션을 수행 중인 상태
            /// </summary>
            DRIVE_DASH = 1L << 3,

            /// <summary>
            /// 가드 액션을 수행 중인 상태
            /// </summary>
            DRIVE_GUARD = 1L << 4,
            
            /// <summary>
            /// 특정 스킬을 수행 중인 상태
            /// </summary>
            DRIVE_SKILL = 1L << 5,
            
            /// <summary>
            /// 체공 : 점프 등으로 공중에 떠서 지면에 도달할 때까지 체공인 상태
            /// </summary>
            FLOAT = 1L << 6,
            
            /// <summary>
            /// 사출 : 외력을 받아 주변 Entity나 벽/장해물/바닥과 상호작용하는 상태
            /// </summary>
            LAUNCH = 1L << 7,

            
            /// <summary>
            /// 축복 : 특정 능력치가 상승한다.
            /// </summary>
            BLESSED = 1L << 10,
            
            /// <summary>
            /// 저지불가 : 피격 모션을 무시한다.
            /// </summary>
            SUPERARMOR = 1L << 11,

            /// <summary>
            /// 불멸 : 데미지를 받지 않는다.
            /// </summary>
            IMMORTAL = 1L << 12,
            
            /// <summary>
            /// 영체화 : 피격모션을 무시하고 데미지도 받지 않으며 적 유닛에게 타게팅 되지도 않는다.
            /// </summary>
            CLOAK = 1L << 13,
            
            
            /// <summary>
            /// 저주 : 특정 능력치가 감소한다.
            /// </summary>
            CURSED = 1L << 20,

            /// <summary>
            /// 경직 : 피격 모션이 재생된다.
            /// </summary>
            STUCK = 1L << 21,
            
            /// <summary>
            /// 감전 : 일정 주기로 감전 데미지를 받는다. 피격시에도 감전 데미지를 받는다. 감전 데미지는 일정 확률로 스턴 상태을 건다.
            /// </summary>
            SHOCK = 1L << 22,
                        
            /// <summary>
            /// 기절 : 피격 모션이 재생된다.
            /// </summary>
            STUN = 1L << 23,
   
            /// <summary>
            /// 출혈 : 일정 주기로 출혈 데미지를 받는다. 이동 상태에서 더 큰 출혈 데미지를 받는다.
            /// </summary>
            BLEED = 1L << 24,
                                                   
            /// <summary>
            /// 상처 : 누적된 만큼 방어력이 감소한다. 상처 상태에서 피격시 출혈을 부여한다.
            /// </summary>
            SCAR = 1L << 25,

            /// <summary>
            /// 중독 : 일정 주기로 중독 데미지를 받는다.
            /// </summary>
            POISON = 1L << 26,

            /// <summary>
            /// 감염 : 감염상태에서 공격하거나 접촉시 대상에게 중독 상태를 건다. 중독 면역이 된다.
            /// </summary>
            INFESTED = 1L << 27,
            
            /// <summary>
            /// 화상 : 일정 주기로 화상 데미지를 받는다. 오한 상태와 빙결 상태에 면역이 된다.
            /// 화상 상태에서 피격시 데미지 일부를 축적하고 n회 축적시 축적한 만큼의 폭발을 일으키고 화상상태를 BURNOUT 상태로 바꾼다.
            /// </summary>
            BURN = 1L << 28,
            
            /// <summary>
            /// 소진 : 화상면역, 피격시 추가 화속성 폭발을 일으키고 데미지를 받는다.
            /// </summary>
            BURNOUT = 1L << 29,
                                 
            /// <summary>
            /// 오한 : 일정 주기로 빙결 데미지를 받는다. 오한 데미지는 n회 누적시 빙결 상태를 건다.
            /// </summary>
            CHILL = 1L << 30,

            /// <summary>
            /// 빙결 : 피격 모션이 재생된다.
            /// </summary>
            FREEZE = 1L << 31,
                       
            /// <summary>
            /// 혼란 : 입력 방향이 반대로 동작한다.
            /// </summary>
            CONFUSE = 1L << 32,
                     
            /// <summary>
            /// 실명 : 시야 스탯을 감소시키고 화면을 가린다.
            /// </summary>
            BLIND = 1L << 33,

            /// <summary>
            /// 침묵 : 스킬을 사용 할 수 없게 된다.
            /// </summary>
            SILENCE = 1L << 34,
            
            /// <summary>
            /// 구속 : 이동/점프/대시를 사용할 수 없게 된다.
            /// </summary>
            BIND = 1L << 35,
                                                          
            /// <summary>
            /// 그로기 : 그로기 모션이 재생된다.
            /// </summary>
            GROGGY = 1L << 36,
            
            /// <summary>
            /// 사망
            /// </summary>
            DEAD = 1L << 37,
            
            
            /// <summary>
            /// 지배 : 특정 대상으로부터 지배를 받아 인공지능을 '펫'으로 교체시킨다.
            /// </summary>
            DOMINATE = 1L << 40,
            
            /// <summary>
            /// 빙의 : 인공지능을 '방황'으로 교체시킨다.
            /// </summary>
            POSSESS = 1L << 41,
                       
            /// <summary>
            /// 버서크 : 모든 동맹 정보를 적대 타입으로 바꾸고 인공지능을 '밀리'로 교체시킨다.
            /// </summary>
            BERSERK = 1L << 42,
            
            
            /// <summary>
            /// 액션 봉인
            /// </summary>
            BLOCK_ACTION = 1L << 51,
            
            /// <summary>
            /// 이동 봉인
            /// </summary>
            BLOCK_MOVE = 1L << 52,
                
            /// <summary>
            /// 절대화 : 피격모션을 무시하고 데미지도 받지 않으며 적 유닛에게 타게팅 되지도 않는다.
            /// 영체화와 달리 이펙트나 연출이 없다.
            /// </summary>
            STABLE = 1L << 53,
            
            /// <summary>
            /// 비활성 : 시스템에 의해 일시적으로 사용불가인 상태
            /// </summary>
            DISABLE = 1L << 54,
            
            
            /* Extend */
            DRIVE_ACTION = DRIVE_DASH | DRIVE_GUARD | DRIVE_SKILL,
            INVINCIBLE = CLOAK | STABLE,
            BLOCK = BLOCK_ACTION | BLOCK_MOVE,
            
            /* GroupMask */
            BlockActionStateGroupMask = SILENCE | BLOCK_ACTION,
            BlockMoveStateGroupMask = BIND | BLOCK_MOVE,
            BlockStateGroupMask = BlockActionStateGroupMask | BlockMoveStateGroupMask,
            AllowStateGroupMask = DRIVE_ORDER | DRIVE_MOVE | DRIVE_ACTION | SUPERARMOR | IMMORTAL | DRIVE_DASH | DRIVE_GUARD
                                | BLESSED | CURSED | SHOCK | BLEED | POISON 
                                | BURN | CHILL | CONFUSE | BLIND | INVINCIBLE,
            StackableStateGroupMask = STUCK | DRIVE_DASH | DRIVE_GUARD | SUPERARMOR 
                                    | BLESSED | CURSED | SHOCK | STUN | BLEED | POISON 
                                    | BURN | CHILL | FREEZE | CONFUSE | BLIND 
                                    | GROGGY | BlockStateGroupMask,
            EffectStateGroupMask = DRIVE_GUARD | SUPERARMOR | IMMORTAL | CLOAK
                                | BLESSED | CURSED | STUN | FREEZE | CONFUSE | BLIND
                                | SILENCE | BIND | GROGGY,
                
            /* PassMask */
            SkillPassMask = BlockMoveStateGroupMask | AllowStateGroupMask,
            AerialSkillPassMask = FLOAT | SkillPassMask,
            MovePassMask = FLOAT | BlockActionStateGroupMask | AllowStateGroupMask,
            JumpPassMask = FLOAT | BlockActionStateGroupMask | AllowStateGroupMask,
            DashPassMask = FLOAT |  BlockActionStateGroupMask | AllowStateGroupMask,
            GuardPassMask = BlockActionStateGroupMask | BlockMoveStateGroupMask | AllowStateGroupMask,
            NavigatePassMask = FLOAT | BlockActionStateGroupMask | AllowStateGroupMask,
            AIUpdatePassMask = FLOAT | BlockActionStateGroupMask | AllowStateGroupMask,
            
            /* FilterMask */
            ValidationFilterMask = DEAD | DISABLE,
            EngageCandidateFilterMask = DEAD | DISABLE | INVINCIBLE,
            ProjectileCollisionFilterMask = DEAD | DISABLE | INVINCIBLE,
            BeamCollisionFilterMask = DEAD | DISABLE | INVINCIBLE,
            HitMotionPlayFilterMask = SUPERARMOR | INVINCIBLE,
            HitMotionStopFilterMask = STUCK | STUN | FREEZE,
        }

        public static EntityStateType[] StateEnumerator;

        public static bool IsStackable(this EntityStateType p_Type) => EntityStateType.StackableStateGroupMask.HasAnyFlagExceptNone(p_Type);
        public static bool HasEffect(this EntityStateType p_Type) => EntityStateType.EffectStateGroupMask.HasAnyFlagExceptNone(p_Type);

        /// <summary>
        /// 게임 개체 랜더 상태를 기술하는 열거형 상수
        /// </summary>
        [Flags]
        public enum GameEntityRenderType
        {
            None = 0,
            
            /// <summary>
            /// 오브젝트 모델를 그림
            /// </summary>
            Model = 1 << 0,
            
            /// <summary>
            /// 오브젝트에 종속된 UI를 그림
            /// </summary>
            UI = 1 << 1,
            
            /// <summary>
            /// 오브젝트에 종속된 서브 오브젝트를 그림
            /// </summary>
            AttachObject = 1 << 2,
        }
        public static GameEntityRenderType[] RenderTypeEnumerator;
        
        /// <summary>
        /// 게임 개체 혹은 게임 개체 간의 이벤트를 기술하는 열거형 상수
        /// </summary>
        [Flags]
        public enum GameEntityBaseEventType : long
        {
            None = 0,

            /// <summary>
            /// 해당 개체의 위치가 변경된 경우
            /// </summary>
            PositionChanged = 1L << 0,
            
            /// <summary>
            /// 해당 개체가 이동한 경우
            /// </summary>
            PositionMoved = 1L << 1,

            /// <summary>
            /// 해당 개체가 오브젝트 풀로 회수된 경우
            /// </summary>
            Retrieved = 1L << 2,

            /// <summary>
            /// 해당 개체가 비활성화된 상태에서 활성화된 경우
            /// </summary>
            Enabled = 1L << 3,
            
            /// <summary>
            /// 해당 개체가 활성화된 상태에서 비활성화된 경우
            /// </summary>
            Disabled = 1L << 4,

            /// <summary>
            /// 해당 개체가 피격된 경우
            /// </summary>
            Hit = 1L << 5,
            
            /// <summary>
            /// 해당 개체가 회복된 경우
            /// </summary>
            Heal = 1L << 6,

            /// <summary>
            /// 해당 개체가 사망/파괴될 경우
            /// </summary>
            TryDead = 1L << 7,
            
            /// <summary>
            /// 해당 개체가 사망/파괴된 경우
            /// </summary>
            Dead = 1L << 8,

            /// <summary>
            /// 다른 개체를 공격한 경우
            /// </summary>
            Strike = 1L << 9,
            
            /// <summary>
            /// 다른 개체를 처치한 경우
            /// </summary>
            Kill = 1L << 10,
            
            
            /// <summary>
            /// 전투 능력치가 변경된 경우
            /// </summary>
            BattleStatus_Change = 1L << 12,
            
            /// <summary>
            /// 보유 스킬 정보가 변경된 경우
            /// </summary>
            Skill_Change = 1L << 13,
            
            /// <summary>
            /// 파티 멤버 변경 시
            /// </summary>
            PartyMember_Change = 1L << 16,
                        
            /// <summary>
            /// 인챈트 정보 변경 시
            /// </summary>
            Enchant_Change = 1L << 17,
            
            /// <summary>
            /// 인챈트 Tick 이벤트 발생 시
            /// </summary>
            Enchant_Tick = 1L << 18,
            
            /// <summary>
            /// 점프 시전 시
            /// </summary>
            JumpUp = 1L << 19,
            
            /// <summary>
            /// 착지 시
            /// </summary>
            ReachGround = 1L << 20,
            
            /// <summary>
            /// 샷 스킬 발동 시
            /// </summary>
            Activate_DefaultCommand = 1L << 21,
            
            /// <summary>
            /// 액티브 스킬 발동 시
            /// </summary>
            Activate_SkillCommand = 1L << 22,
            
            /// <summary>
            /// 패시브 스킬 발동 시
            /// </summary>
            Activate_Passive = 1L << 23,
            
            /// <summary>
            /// 아이템 사용 시
            /// </summary>
            Activate_Item = 1L << 24,
            
            /// <summary>
            /// 레벨 변경 시
            /// </summary>
            Level_Change = 1L << 25,
            
            /// <summary>
            /// 상태가 갱신된 경우
            /// </summary>
            State_Change = 1L << 26,
            
            /// <summary>
            /// 페이즈가 갱신된 경우
            /// </summary>
            Phase_Change = 1L << 27,
            
            /// <summary>
            /// 아이템 사용 정보가 변경된 경우
            /// </summary>
            ItemUse = 1L << 28,
            
            /// <summary>
            /// 인벤토리 정보가 변경된 경우
            /// </summary>
            Inventory_Change = 1L << 29,
        }
        public static GameEntityBaseEventType[] GameEntityBaseEventTypeEnumerator;

        /// <summary>
        /// 게임 개체에서 UI로의 이벤트를 기술하는 열거형 상수
        /// </summary>
        [Flags]
        public enum GameEntityUIEventType
        {
            None = 0,
            
            /// <summary>
            /// 개체의 이름이 변경된 경우
            /// </summary>
            ChangeName = 1 << 1,
            
            /// <summary>
            /// 개체의 이름 색상이 변경된 경우
            /// </summary>
            ChangeNameColor = 1 << 2,

            /// <summary>
            /// 개체의 이름 심볼이 변경된 경우
            /// </summary>
            ChangeNameSymbol = 1 << 3,
        }
        public static GameEntityUIEventType[] GameEntityUIEventTypeEnumerator;
        
        /// <summary>
        /// 게임 개체 내부의 모듈 간 이벤트를 기술하는 열거형 상수
        /// </summary>
        [Flags]
        public enum GameEntityModuleEventType
        {
            None = 0,
        }
        public static GameEntityModuleEventType[] GameEntityInnerModuleEventTypeEnumerator;

        /// <summary>
        /// 업데이트 마지막에 처리할 이벤트 타입
        /// </summary>
        [Flags]
        public enum LateEventType
        {
            None = 0,
            
            /// <summary>
            /// 위치를 갱신해야 하는 경우
            /// </summary>
            UpdateEntityPosition = 1 << 0,
     
            /// <summary>
            /// 카메라 상호작용
            /// </summary>
            UpdateCameraInteract = 1 << 1,
            
            /// <summary>
            /// 스킬 변경 이벤트 지연 전파
            /// </summary>
            UpdateSkillChange = 1 << 2,
            
            /// <summary>
            /// 능력치 변화
            /// </summary>
            BaseStatusChange = 1 << 6,
            
            /// <summary>
            /// 능력치 변화
            /// </summary>
            BattleStatusChange = 1 << 7,
            
            /// <summary>
            /// 능력치 변화
            /// </summary>
            ShotStatusChange = 1 << 8,
            
            /// <summary>
            /// 풀링
            /// </summary>
            Pooling = 1 << 9,
            
            /// <summary>
            /// 인벤토리 변화
            /// </summary>
            InventoryUpdate = 1 << 10,
        }
         
        public static LateEventType[] LateEventTypeEnumeraotr;
        
        /// <summary>
        /// 개체가 소속된 그룹 타입
        /// </summary>
        [Flags]
        public enum GameEntityGroupType 
        {
            /// <summary>
            /// 세력 없음
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 플레이어 세력 0
            /// </summary>
            PlayerForce0 = 1 << 0,
            
            /// <summary>
            /// 플레이어 세력 1
            /// </summary>
            PlayerForce1 = 1 << 1,
            
            /// <summary>
            /// 플레이어 세력 2
            /// </summary>
            PlayerForce2 = 1 << 2,
            
            /// <summary>
            /// 플레이어 세력 3
            /// </summary>
            PlayerForce3 = 1 << 3,
            
            /// <summary>
            /// 플레이어 세력 4
            /// </summary>
            PlayerForce4 = 1 << 4,
            
            /// <summary>
            /// 플레이어 세력 5
            /// </summary>
            PlayerForce5 = 1 << 5,
            
            /// <summary>
            /// 플레이어 세력 6
            /// </summary>
            PlayerForce6 = 1 << 6,
            
            /// <summary>
            /// 플레이어 세력 7
            /// </summary>
            PlayerForce7 = 1 << 7,

            /// <summary>
            /// 모든 플레이어 세력 마스크
            /// </summary>
            PlayerForce = PlayerForce0 | PlayerForce1 | PlayerForce2 | PlayerForce3 | PlayerForce4 | PlayerForce5 | PlayerForce6 | PlayerForce7,
            
            /// <summary>
            /// 아군 컴퓨터 세력 0
            /// </summary>
            AllyComputerForce0 = 1 << 10,
            
            /// <summary>
            /// 아군 컴퓨터 세력 1
            /// </summary>
            AllyComputerForce1 = 1 << 11,
            
            /// <summary>
            /// 아군 컴퓨터 세력 2
            /// </summary>
            AllyComputerForce2 = 1 << 12,
            
            /// <summary>
            /// 아군 컴퓨터 세력 3
            /// </summary>
            AllyComputerForce3 = 1 << 13,
            
            /// <summary>
            /// 아군 컴퓨터 세력 4
            /// </summary>
            AllyComputerForce4 = 1 << 14,
            
            /// <summary>
            /// 아군 컴퓨터 세력 5
            /// </summary>
            AllyComputerForce5 = 1 << 15,
            
            /// <summary>
            /// 아군 컴퓨터 세력 6
            /// </summary>
            AllyComputerForce6 = 1 << 16,
            
            /// <summary>
            /// 아군 컴퓨터 세력 7
            /// </summary>
            AllyComputerForce7 = 1 << 17,
            
            /// <summary>
            /// 모든 아군 컴퓨터 세력 마스크
            /// </summary>
            AllyComputerForce = AllyComputerForce0 | AllyComputerForce1 | AllyComputerForce2 | AllyComputerForce3 | AllyComputerForce4 | AllyComputerForce5 | AllyComputerForce6 | AllyComputerForce7,
            
            /// <summary>
            /// 적대 컴퓨터 세력 0
            /// </summary>
            EnemyComputerForce0 = 1 << 20,
            
            /// <summary>
            /// 적대 컴퓨터 세력 1
            /// </summary>
            EnemyComputerForce1 = 1 << 21,
            
            /// <summary>
            /// 적대 컴퓨터 세력 2
            /// </summary>
            EnemyComputerForce2 = 1 << 22,
            
            /// <summary>
            /// 적대 컴퓨터 세력 3
            /// </summary>
            EnemyComputerForce3 = 1 << 23,
            
            /// <summary>
            /// 적대 컴퓨터 세력 4
            /// </summary>
            EnemyComputerForce4 = 1 << 24,
            
            /// <summary>
            /// 적대 컴퓨터 세력 5
            /// </summary>
            EnemyComputerForce5 = 1 << 25,
            
            /// <summary>
            /// 적대 컴퓨터 세력 6
            /// </summary>
            EnemyComputerForce6 = 1 << 26,
            
            /// <summary>
            /// 적대 컴퓨터 세력 7
            /// </summary>
            EnemyComputerForce7 = 1 << 27,
                      
            /// <summary>
            /// 모든 적대 컴퓨터 세력 마스크
            /// </summary>
            EnemyComputerForce = EnemyComputerForce0 | EnemyComputerForce1 | EnemyComputerForce2 | EnemyComputerForce3 | EnemyComputerForce4 | EnemyComputerForce5 | EnemyComputerForce6 | EnemyComputerForce7,
            
            EveryGroup = ~None,
        }
        
        /// <summary>
        /// 개체 간의 동맹 관계 타입
        /// </summary>
        [Flags]
        public enum GameEntityGroupRelateType
        {
            None = 0,
            Enemy = 1 << 0,
            Ally = 1 << 1,
            Neutral = 1 << 2,
        }
        
        /// <summary>
        /// 개체가 서로를 인식하는 상태 종류 타입
        /// </summary>
        [Flags]
        public enum GameEntityObservingState
        {
            None = 0,
            Upper = 1 << 0,
            Lower = 1 << 1,
            EachOther = Upper | Lower,
        }
        
        #endregion
    }
}