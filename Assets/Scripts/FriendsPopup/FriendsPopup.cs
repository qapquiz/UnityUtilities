using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Undefined.FriendsPopup {
	public class FriendsPopup : MonoBehaviour {

		[Header("Basic")]
		[SerializeField]
		private Text m_textHeader;

		public Text TextHeader {
			set {
				m_textHeader = value;
			}
			get {
				return m_textHeader;
			}
		}
		
		[Header("Tabs")]
		[SerializeField]
		private GameObject m_tabFriendsList;
		public GameObject TabFriendsList {
			set {
				m_tabFriendsList = value;
			}
			get {
				return m_tabFriendsList;
			}
		}

		[SerializeField]
		private GameObject m_tabFriendsReference;
		public GameObject TabFriendsReference {
			set {
				m_tabFriendsReference = value;
			}
			get {
				return m_tabFriendsReference;
			}
		}

		[SerializeField]
		private Sprite m_normalSpriteTab;
		[SerializeField]
		private Sprite m_highlightSpriteTab;

		private Toggle m_tabFriendsListToggle;
		public Toggle TabFriendsListToggle{
			set {
				m_tabFriendsListToggle = value;
			} 

			get {
				return m_tabFriendsListToggle;
			}
		}

		private Toggle m_tabFriendsReferenceToggle;
		public Toggle TabFriendsReferenceToggle{
			set {
				m_tabFriendsReferenceToggle = value;
			}

			get {
				return m_tabFriendsReferenceToggle;
			}
		}

		private Image m_tabFriendsListImage;
		public Image TabFriendsListImage {
			set {
				m_tabFriendsListImage = value;
			}
		}
		

		private Image m_tabFriendsReferenceImage;
		public Image TabFriendsReferenceImage{
			set {
				m_tabFriendsReferenceImage = value;
			}
		}

		[Header("FriendsList")]
		[SerializeField]
		private GameObject m_friendsList;
		public GameObject FriendsList {
			set {
				m_friendsList = value;
			}
		}
		
		[SerializeField]
		private GameObject m_noFriendsText;
		public GameObject NoFriendsText {
			set {
				m_noFriendsText = value;
			}
			get {
				return m_noFriendsText;
			}
		}

		[SerializeField]
		private GameObject m_friendsScrollView;

		public GameObject FriendsScrollView {
			set {
				m_friendsScrollView = value;
			}
			get {
				return m_friendsScrollView;
			}
		}

		[Header("FriendsReference")]
		[SerializeField]
		private GameObject m_friendsReference;
		public GameObject FriendsReference {
			set {
				m_friendsReference = value;
			}
		}

		[SerializeField]
		private GameObject m_friendsReferenceActive;
		public GameObject FriendsReferenceActive {
			set {
				m_friendsReferenceActive = value;
			}

			get {
				return m_friendsReferenceActive;
			}
		}

		[SerializeField]
		private GameObject m_friendsReferenceInactive;
		public GameObject FriendsReferenceInactive {
			set {
				m_friendsReferenceInactive = value;
			}

			get {
				return m_friendsReferenceInactive;
			}
		}


		[SerializeField]
		private GameObject m_friendsReferenceNew;
		public GameObject FriendsReferenceNew {
			set {
				m_friendsReferenceNew = value;
			}
			
			get {
				return m_friendsReferenceNew;
			}
		}
		

		private State handler(State state, Action action) {
			switch (action.ActionType) {
				case ActionType.Init:
					state = new State(new Basic(
						header: ""
					), new FriendsList(
						mode: Mode.Normal,
						friends: GetFriendsFromUser(),
						isShowFriendsList: GetFriendsFromUser().Count > 0
					), new FriendsReference(

					));

					UpdateHeader(state.Basic.Header);
					
					break;

				case ActionType.OpenFriendsList:
					ActionOpenFriendsList actionOpenFriendsList = (ActionOpenFriendsList) action;
					state.Basic.Header = actionOpenFriendsList.Header;

					m_tabFriendsListToggle.isOn = true;
					m_tabFriendsReferenceToggle.isOn = false;

					
					m_friendsReference.SetActive(false);
					m_friendsList.SetActive(true);

					UpdateHeader(state.Basic.Header);
					ResizeTextHeader(width: 169f, height: 48.2f);

					if (state.FriendsList.IsShowFriendsList){
						HideNoFriendsText();
						ShowFriendsScrollView();
					} else {
						ShowNoFriendsText();
						HideFriendsScrollView();
					}
					break;
				
				case ActionType.OpenFriendsReference:
					ActionOpenFriendsReference actionOpenFriendsReference = (ActionOpenFriendsReference) action;
					state.Basic.Header = actionOpenFriendsReference.Header;
					state.FriendsReference.Status = actionOpenFriendsReference.Status;

					m_tabFriendsListToggle.isOn = false;
					m_tabFriendsReferenceToggle.isOn = true;

					m_friendsList.SetActive(false);
					m_friendsReference.SetActive(true);

					switch (state.FriendsReference.Status) {
						case FriendsReferenceStatus.Active:
							m_friendsReferenceActive.SetActive(true);
							m_friendsReferenceInactive.SetActive(false);
							m_friendsReferenceNew.SetActive(false);
							break;
						case FriendsReferenceStatus.Inactive:
							m_friendsReferenceInactive.SetActive(true);
							m_friendsReferenceActive.SetActive(false);
							m_friendsReferenceNew.SetActive(false);
							break;
						case FriendsReferenceStatus.New:
							m_friendsReferenceNew.SetActive(true);
							m_friendsReferenceActive.SetActive(false);
							m_friendsReferenceInactive.SetActive(false);
							break;
					}

					UpdateHeader(state.Basic.Header);
					ResizeTextHeader(width: 320f, height: 48.2f);
					break;

				default:
				  break;
			}

			return state;
		}

		private Store m_friendsStore;
		public Store Store {
			get {
				if (m_friendsStore == null) {
					m_friendsStore = CreateStore(handler);
				}

				return m_friendsStore;
			}
		}

		void Start() {
			Init();
		}

		private void Init() {
			m_friendsStore = CreateStore(handler);

			PrepareTabs();

			m_friendsStore.Dispatch(new Action() {
				ActionType = ActionType.Init
			});
			m_friendsStore.Dispatch(new ActionOpenFriendsReference() {
				ActionType = ActionType.OpenFriendsReference,
				Header = "รางวัลกิจกรรมชวนเพื่อน",
				Status = FriendsReferenceStatus.Active
			});
			m_friendsStore.Dispatch(new ActionOpenFriendsList() {
				ActionType = ActionType.OpenFriendsList,
				Header = $"เพื่อน {GetFriendsFromUser().Count}/{GetFriendLimits()}"
			});
		}

		private void PrepareTabs() {
			m_tabFriendsListToggle = m_tabFriendsList.GetComponent<Toggle>();
			m_tabFriendsListImage = m_tabFriendsList.GetComponent<Image>();

			m_tabFriendsReferenceToggle = m_tabFriendsReference.GetComponent<Toggle>();
			m_tabFriendsReferenceImage = m_tabFriendsReference.GetComponent<Image>();
		}

		public void ToggleFriendsListTab(bool isToggle) {
			if (isToggle) {
				m_tabFriendsListImage.sprite = m_highlightSpriteTab;
				m_friendsStore.Dispatch(new ActionOpenFriendsList() {
					ActionType = ActionType.OpenFriendsList,
					Header = $"เพื่อน {GetFriendsFromUser().Count}/{GetFriendLimits()}"
				});
			} else {
				m_tabFriendsListImage.sprite = m_normalSpriteTab;
			}
		}

		public void ToggleFriendsReference(bool isToggle) {
			if (isToggle) {
				m_tabFriendsReferenceImage.sprite = m_highlightSpriteTab;
				m_friendsStore.Dispatch(new ActionOpenFriendsReference() {
					ActionType = ActionType.OpenFriendsReference,
					Header = "รางวัลสำหรับเพื่อนเก่า",
					Status = FriendsReferenceStatus.New
				});
			} else {
				m_tabFriendsReferenceImage.sprite = m_normalSpriteTab;
			}
		}

		private Store CreateStore(Func<State, Action, State> handler)
			=> new Store(reducer: new Reducer(handler: handler));

		public void UpdateHeader(string header) {
			m_textHeader.text = header;
		}

		public void ResizeTextHeader(float width, float height) {
			m_textHeader.rectTransform.sizeDelta = new Vector2(width, height);
		}

		public List<Friend> GetFriendsFromUser()
		  => User.GetSingleton().friends;

		public int GetFriendLimits()
		  => User.GetSingleton().m_friendLimits;

		public void ShowNoFriendsText() {
			m_noFriendsText.SetActive(true);
		}

		public void HideNoFriendsText() {
			m_noFriendsText.SetActive(false);
		}

		public void ShowFriendsScrollView() {
			m_friendsScrollView.SetActive(true);
		}

		public void HideFriendsScrollView() {
			m_friendsScrollView.SetActive(false);
		}
	}
}

