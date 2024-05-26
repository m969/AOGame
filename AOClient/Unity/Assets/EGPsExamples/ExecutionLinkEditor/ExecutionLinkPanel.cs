using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using EGamePlay.Combat;
using AO;

namespace EGamePlay
{
    public class ExecutionLinkPanel : MonoBehaviour
    {
		public static ExecutionLinkPanel Instance { get; set; }
        public Text SkillTimeText;
        public Text SkillNameText;
        public Text SkillDescText;
        public Image SkillTimeImage;
        public Transform TimeCursorTrm;
        [Space(10)]
        public Transform FrameInfosContentTrm;
        public Transform FrameTrm;
        public Transform FrameTextTrm;
        public Vector2 FrameTextPos { get; set; }
        [Space(10)]
        public Transform TrackListTrm;
        public Transform TrackTrm;
        [Space(10)]
        public Transform RightContextTrm;
        public Button NewExecutionBtn;
        public Button AddClipBtn;
        public Button SaveBtn;
        public Button DeleteClipBtn;
        public Toggle PauseToggle;
		public Button PlayButton;
        public Button ReloadButton;
        public Button StepBtn;
        public Transform ContentTrm;
        public Transform Button;

        //public Unit Unit { get; set; }
        //public Unit Monster { get; set; }
        //public CastConfig CurrentSkillConfig { get; set; }
        public int CurrentSkillId { get; set; }
        public float TotalTime { get; set; }
        public float CurrentTime { get; set; }
        public int NextActionIndex { get; set; }
        public float PanelWidth { get; set; }
        public bool IsPlaying { get; set; }
        public string CurrentExecutionAssetPath { get; set; }
		public ExecutionObject CurrentExecutionObject { get; set; }
		public ExecuteClipData CurrentExecutionClip { get; set; }
		public CombatEntity HeroEntity { get; set; }
        public CombatEntity BossEntity { get; set; }


        // Start is called before the first frame update
        void Start()
        {
			Instance = this;

			var r = GetComponent<RectTransform>();
            PanelWidth = Screen.width - r.offsetMin.x + r.offsetMax.x;
            //PanelWidth = GetComponent<RectTransform>().sizeDelta.x;
			//Log.Debug($"{GetComponent<RectTransform>().offsetMin} {GetComponent<RectTransform>().offsetMax}");

            //Button.SetParent(null);
            TrackTrm.SetParent(null);

            FrameTextPos = FrameTextTrm.GetComponent<RectTransform>().localPosition;
            FrameTrm.SetParent(null);
            FrameTextTrm.SetParent(null);

            SkillTimeImage.fillAmount = 0;
			TimeCursorTrm.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);

			PlayButton.onClick.AddListener(PlaySkillExecution);

			NewExecutionBtn.onClick.AddListener(NewExecutionAsset);
			AddClipBtn.onClick.AddListener(AddClipAsset);
			DeleteClipBtn.onClick.AddListener(DeleteClipAsset);
			SaveBtn.onClick.AddListener(SaveAsset);

            Invoke(nameof(AfterStart), 1f);
        }

		private void AfterStart()
		{
			HeroEntity = AO.Actor.Main.GetComponent<UnitCombatComponent>().CombatEntity;
			HeroEntity.IsHero = true;
			HeroEntity.ModelTrans = AO.Actor.Main.GetComponent<UnitViewComponent>().UnitObj.transform;
			var bossUnit = AOGame.ClientApp.GetComponent<ExecutionEditorModeComponent>().BossUnit;
            BossEntity = bossUnit.GetComponent<UnitCombatComponent>().CombatEntity;
            BossEntity.ModelTrans = bossUnit.GetComponent<UnitViewComponent>().UnitObj.transform;
            //Monster.Boss.MotionComponent.Enable = false;
            //Monster.Boss.AnimationComponent.Speed = 1;
            //Monster.Boss.AnimationComponent.TryPlayFade(Monster.Boss.AnimationComponent.IdleAnimation);
        }

        // Update is called once per frame
        void Update()
		{
			if (IsPlaying)
			{
				CurrentTime += Time.deltaTime;
				//Log.Debug($"ExecutionLinkPanel {CurrentTime}");

				var perc = CurrentTime / TotalTime;
				SkillTimeImage.fillAmount = perc;
				TimeCursorTrm.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(SkillTimeImage.fillAmount * PanelWidth, 0, 0);

				if (SkillTimeImage.fillAmount >= 1)
				{
					IsPlaying = false;
					PlayButton.GetComponentInChildren<Text>().text = "����";
				}
			}

			if (Input.GetMouseButtonUp((int)UnityEngine.UIElements.MouseButton.LeftMouse))
			{
				RightContextTrm.gameObject.SetActive(false);
			}
		}

		public void NewExecutionAsset()
		{
			var assetName = "Execution_";
			var i = 0;
			while (true)
			{
				var newAssetName = assetName;
                if (i > 0)
				{
					newAssetName = assetName + i;
                }
				var asset = AssetDatabase.LoadAssetAtPath<ExecutionObject>($"Assets/{newAssetName}.asset");
				if (asset == null)
				{
                    assetName = newAssetName;
                    break;
				}
				i++;
            }

            CurrentExecutionAssetPath = $"Assets/{assetName}.asset";
			var excObj = ScriptableObject.CreateInstance<ExecutionObject>();
			excObj.TotalTime = 1.5f;
			//excObj.name = i.ToString();
            AssetDatabase.CreateAsset(excObj, CurrentExecutionAssetPath);
			SkillListPanel.Instance.RefreshList();
			LoadCurrentSkill();
        }

        public void AddClipAsset()
		{
			if (string.IsNullOrEmpty(CurrentExecutionAssetPath))
			{
				return;
			}
			var excObj = AssetDatabase.LoadAssetAtPath<ExecutionObject>(CurrentExecutionAssetPath);
			var excClipObj = ScriptableObject.CreateInstance<ExecuteClipData>();
			excClipObj.name = "ExecuteClip";
			excClipObj.ExecuteClipType = ExecuteClipType.CollisionExecute;
			excClipObj.CollisionExecuteData = new CollisionExecuteData();
			excClipObj.GetClipTime().EndTime = 0.1f;
            excObj.ExecuteClips.Add(excClipObj);
			AssetDatabase.AddObjectToAsset(excClipObj, excObj);
			EditorUtility.SetDirty(excObj);
            AssetDatabase.SaveAssetIfDirty(excObj);
            LoadCurrentSkill();
        }

        public void DeleteClipAsset()
        {
			//Log.Debug($"DeleteClipAsset {CurrentExecutionClip} {CurrentExecutionObject.ExecutionClips.Count}");
			CurrentExecutionObject.ExecuteClips.Remove(CurrentExecutionClip);
            AssetDatabase.RemoveObjectFromAsset(CurrentExecutionClip);
            EditorUtility.SetDirty(CurrentExecutionObject);
            AssetDatabase.SaveAssetIfDirty(CurrentExecutionObject);
            LoadCurrentSkill();
			RightContextTrm.gameObject.SetActive(false);
		}

        public void SaveAsset()
        {
			if (CurrentExecutionObject == null)
			{
				return;
			}
            //foreach (var item in CurrentExecutionObject.ExecutionClips)
            //{
            //	AssetDatabase.isd
            //}
            EditorUtility.SetDirty(CurrentExecutionObject);
            AssetDatabase.SaveAssetIfDirty(CurrentExecutionObject);
        }

        T Load<T>(string asset) where T : Object
        {
			return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(asset);
        }

		public static void DestroyChildren(Transform transform)
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				var child = transform.GetChild(i);
				GameObject.DestroyImmediate(child.gameObject);
			}
		}

		public void LoadSkill(string path)
		{
            CurrentExecutionAssetPath = path;
            LoadCurrentSkill();
        }

		public void LoadCurrentSkill()
        {
			var self = this;
			DestroyChildren(self.TrackListTrm);

			CurrentExecutionObject = AssetDatabase.LoadAssetAtPath<ExecutionObject>(CurrentExecutionAssetPath);

			//var cast = CastConfigCategory.Instance.Get(self.CurrentSkillId);
			//self.CurrentSkillConfig = cast;

			//if (cast == null)
			//{
			//	return;
			//}

			//var time = 1500;
			self.TotalTime = (float)CurrentExecutionObject.TotalTime;

			//self.SkillNameText.text = $"{cast.Id}_{cast.Name}";
			//self.SkillTimeImage.GetComponentInChildren<Text>().text = $"{self.TotalTime}��";

			foreach (var item in CurrentExecutionObject.ExecuteClips)
            {
				if (item.ExecuteClipType == ExecuteClipType.CollisionExecute)
				{
					self.LoadCurrentSkillCollisionClip(item);
				}
				if (item.ExecuteClipType == ExecuteClipType.ActionEvent)
				{
					self.LoadCurrentSkillActionEvent(item);
				}
                if (item.ExecuteClipType == ExecuteClipType.Animation)
                {
                    self.LoadCurrentSkillAnimation(item);
                }
                if (item.ExecuteClipType == ExecuteClipType.ParticleEffect)
                {
                    self.LoadCurrentSkillParticleEffect(item);
                }
            }
            //self.LoadCurrentSkillAction(cast.HitAction, "action");
            //self.LoadCurrentSkillAction(new int[] { 500 }, "buff");
			//self.LoadCurrentSkillSound();

			var trackListSize = self.TrackListTrm.rectTransform().sizeDelta;
			var space = self.TrackListTrm.GetComponent<VerticalLayoutGroup>().spacing;
			self.TrackListTrm.rectTransform().sizeDelta = new Vector2(trackListSize.x, self.TrackListTrm.childCount * (self.TrackTrm.rectTransform().sizeDelta.y + space));

			DestroyChildren(self.FrameInfosContentTrm);

			var frameCount = (int)(self.TotalTime * 100);
			for (int i = 0; i < frameCount; i++)
			{
				var frameObj = GameObject.Instantiate(self.FrameTrm, self.FrameInfosContentTrm);
				if (i % 5 != 0)
				{
					var c = frameObj.GetComponent<Image>().color;
					frameObj.GetComponent<Image>().color = new Color(c.r, c.g, c.b, .5f);
					var r = frameObj.rectTransform();
					r.sizeDelta = new Vector2(r.sizeDelta.x, r.sizeDelta.y * 0.4f);
				}
				else
				{
					if (i % 10 != 0)
					{
						var c = frameObj.GetComponent<Image>().color;
						var r = frameObj.rectTransform();
						r.sizeDelta = new Vector2(r.sizeDelta.x, r.sizeDelta.y * 0.8f);
					}
					else
					{
						var textObj = GameObject.Instantiate(self.FrameTextTrm, frameObj.transform);
						textObj.rectTransform().localPosition = self.FrameTextPos;
						var milis = i * 10;
						var secs = milis / 1000;
						var secs2 = milis % 1000 / 10;
						textObj.GetComponent<Text>().text = $"{secs}:{secs2.ToString().PadLeft(2, '0')}";
					}
				}
			}
		}

		void LoadCurrentSkillCollisionClip(ExecuteClipData trackClipData)
		{
			var self = this;
			//var cast = self.CurrentSkillConfig;
			//var execution = "execution";
			//if (string.IsNullOrEmpty(execution))
			//{
			//	return;
			//}
			var animTrack = GameObject.Instantiate(self.TrackTrm);
			animTrack.SetParent(self.TrackListTrm);
			animTrack.GetComponentInChildren<Text>().text = "collision execute";
			//var animLength = trackClipData.ExecutionClipData.EndTime;

			//var trackClipData = new TrackClipData();
			//trackClipData.TrackClipType = TrackClipType.ExecutionClip;
			//trackClipData.ExecutionClipData = new ExecutionClipData();
			//trackClipData.ExecutionClipData._StartTime = 0;
			//trackClipData.ExecutionClipData._EndTime = animLength;
			trackClipData.TotalTime = self.TotalTime;

			var trackClip = animTrack.GetComponentInChildren<TrackClip>();
			trackClip.SetClipType(trackClipData);
		}

		void LoadCurrentSkillAnimation(ExecuteClipData trackClipData)
		{
			var self = this;
			var anim = "anim";
			if (string.IsNullOrEmpty(anim))
			{
				return;
			}
			if (trackClipData.AnimationData.AnimationClip != null)
			{
				anim = trackClipData.AnimationData.AnimationClip.name;
            }
			var animTrack = GameObject.Instantiate(self.TrackTrm);
			animTrack.SetParent(self.TrackListTrm);
			animTrack.GetComponentInChildren<Text>().text = $"animation clip : {anim}";
			
            trackClipData.TotalTime = self.TotalTime;

            var trackClip = animTrack.GetComponentInChildren<TrackClip>();
			trackClip.SetClipType(trackClipData);
		}

        void LoadCurrentSkillParticleEffect(ExecuteClipData trackClipData)
        {
            var self = this;

			var name = "";
			if (trackClipData.ParticleEffectData.ParticleEffect != null)
			{
                name = trackClipData.ParticleEffectData.ParticleEffect.name;
            }
            var animTrack = GameObject.Instantiate(self.TrackTrm);
            animTrack.SetParent(self.TrackListTrm);
            animTrack.GetComponentInChildren<Text>().text = $"{name}";

            trackClipData.TotalTime = self.TotalTime;

            var trackClip = animTrack.GetComponentInChildren<TrackClip>();
            trackClip.SetClipType(trackClipData);
        }

        void LoadCurrentSkillSound()
		{
			var self = this;
			//var cast = self.CurrentSkillConfig;
			var sound = "sound";
			if (string.IsNullOrEmpty(sound))
			{
				return;
			}
			var audio = Load<AudioClip>(sound);
			var animTrack = GameObject.Instantiate(self.TrackTrm);
			animTrack.SetParent(self.TrackListTrm);
			animTrack.GetComponentInChildren<Text>().text = $"{sound}";

			var trackClipData = new ExecuteClipData();
			trackClipData.TotalTime = self.TotalTime;
			trackClipData.ExecuteClipType = ExecuteClipType.Audio;
			trackClipData.AudioData = new AudioData();
			trackClipData.AudioData.AudioClip = audio;
			trackClipData.StartTime = 0;
			trackClipData.EndTime = audio.length;

			var trackClip = animTrack.GetComponentInChildren<TrackClip>();
			trackClip.SetClipType(trackClipData);
			trackClip.SetDragEvent();
			trackClip.SliderRight.value = audio.length;
			trackClip.ClipTypeBar.color = new Color(204 / 255f, 130 / 255f, 0f, 1);
		}

		void LoadCurrentSkillActionEvent(ExecuteClipData trackClipData)
		{
			var self = this;
            var startTime = trackClipData.StartTime;

            var actionTrack = GameObject.Instantiate(self.TrackTrm);
            actionTrack.SetParent(self.TrackListTrm);
			if (trackClipData.ActionEventData != null)
			{
				actionTrack.GetComponentInChildren<Text>().text = "";//$"{trackClipData.ActionEventData.NewExecution}";
			}

            var trackClip = actionTrack.GetComponentInChildren<TrackClip>();

            trackClip.SliderLeft.value = (float)startTime / self.TotalTime;
            trackClip.SliderRight.value = (float)startTime / self.TotalTime + 0.01f;
            trackClipData.TotalTime = self.TotalTime;
            trackClip.DisableSlider();
            trackClip.SetClipType(trackClipData);
        }

        public void PlaySkillExecution()
        {
            if (CurrentExecutionObject == null)
            {
                return;
            }

            //#if !EGAMEPLAY_EXCEL
            SkillTimeImage.fillAmount = 0;
            CurrentTime = 0;
            IsPlaying = true;
            if (CurrentExecutionObject.AbilityId > 0 && HeroEntity.IdSkills.TryGetValue(CurrentExecutionObject.AbilityId, out var skillAbility))
            {
                skillAbility.LoadExecution();
                if (CurrentExecutionObject.TargetInputType == ExecutionTargetInputType.Target)
                {
                    if (skillAbility.SkillEffectsConfig.AffectTargetType == SkillAffectTargetType.EnemyTeam)
                    {
                        HeroEntity.GetComponent<SpellComponent>().SpellWithTarget(skillAbility, BossEntity);
                    }
                    else
                    {
                        HeroEntity.GetComponent<SpellComponent>().SpellWithTarget(skillAbility, HeroEntity);
                    }
                }
                if (CurrentExecutionObject.TargetInputType == ExecutionTargetInputType.Point)
                {
                    HeroEntity.GetComponent<SpellComponent>().SpellWithPoint(skillAbility, BossEntity.Position);
                }
            }
            else
            {
                HeroEntity.ModelTrans.localRotation = Quaternion.LookRotation(BossEntity.Position - HeroEntity.Position);
                //var skillAbility = Hero.Instance.CombatEntity.AttachSkill(new SkillConfigObject() { Id = 9999 });
                if (CurrentExecutionObject.TargetInputType == ExecutionTargetInputType.Target)
                {
                    var execution = HeroEntity.AddChild<SkillExecution>(null);
                    execution.ExecutionObject = CurrentExecutionObject;
                    execution.InputTarget = BossEntity;
                    execution.LoadExecutionEffects();
                    execution.BeginExecute();
                    execution.AddComponent<UpdateComponent>();
                }
                if (CurrentExecutionObject.TargetInputType == ExecutionTargetInputType.Point)
                {
                    var execution = HeroEntity.AddChild<SkillExecution>(null);
                    execution.ExecutionObject = CurrentExecutionObject;
                    execution.InputPoint = BossEntity.Position;
                    execution.LoadExecutionEffects();
                    execution.BeginExecute();
                    execution.AddComponent<UpdateComponent>();
                }
            }
            //#endif
        }
    }

    public static class ExecutionLinkPanelEx
    {
		public static RectTransform rectTransform(this Transform transform)
		{
			return transform.GetComponent<RectTransform>();
		}

		public static RectTransform rectTransform(this GameObject transform)
		{
			return transform.GetComponent<RectTransform>();
		}

        public static void DestroyChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }
}
