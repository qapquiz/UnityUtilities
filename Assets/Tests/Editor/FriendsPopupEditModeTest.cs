using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Undefined.FriendsPopup;

[Category("FriendsPopup")]
public class FriendsPopupEditModeTest {

	GameObject friendsPopup;
	FriendsPopup friendsPopupScript;

	[SetUp]
	public void Setup() {
		friendsPopup = new GameObject("FriendsPopup");

		friendsPopupScript = friendsPopup.AddComponent<FriendsPopup> ();
		Text textHeader = friendsPopup.AddComponent<Text>();
		friendsPopupScript.TextHeader = textHeader;

		friendsPopupScript.NoFriendsText = new GameObject();
		friendsPopupScript.FriendsScrollView = new GameObject();
		
		friendsPopupScript.TabFriendsList = new GameObject();
		friendsPopupScript.TabFriendsListToggle = friendsPopupScript.TabFriendsList.AddComponent<Toggle>();
		friendsPopupScript.TabFriendsListImage = friendsPopupScript.TabFriendsList.AddComponent<Image>();

		friendsPopupScript.TabFriendsReference = new GameObject();
		friendsPopupScript.TabFriendsReferenceToggle = friendsPopupScript.TabFriendsReference.AddComponent<Toggle>();
		friendsPopupScript.TabFriendsReferenceImage = friendsPopupScript.TabFriendsReference.AddComponent<Image>();

		friendsPopupScript.FriendsList = new GameObject();

		friendsPopupScript.FriendsReference = new GameObject();
		friendsPopupScript.FriendsReferenceActive = new GameObject();
		friendsPopupScript.FriendsReferenceInactive = new GameObject();
		friendsPopupScript.FriendsReferenceNew = new GameObject();
	}

	[Test]
	public void FriendsPopupTestDispatchInitEditModeTestPasses() {
		friendsPopupScript.Store.Dispatch(new Action() {
			ActionType = ActionType.Init
		});

		StringAssert.AreEqualIgnoringCase("", friendsPopupScript.Store.State.Basic.Header);
		StringAssert.AreEqualIgnoringCase("", friendsPopupScript.TextHeader.text);
	}

	[Test]
	[TestCase("รางวัลกิจกรรมชวนเพื่อน", FriendsReferenceStatus.Active)]
	[TestCase("รางวัลสำหรับผู้เล่นเก่า", FriendsReferenceStatus.Inactive)]
	[TestCase("รางวัลสำหรับผู้เล่นใหม่", FriendsReferenceStatus.New)]
	public void FriendsPopupTestDispatchOpenFriendReferenceEditModeTestPasses(string header, FriendsReferenceStatus status) {
		friendsPopupScript.Store.Dispatch(new ActionOpenFriendsReference() {
			ActionType = ActionType.OpenFriendsReference,
			Header = header,
			Status = status
		});

		StringAssert.AreEqualIgnoringCase(header, friendsPopupScript.Store.State.Basic.Header);
		StringAssert.AreEqualIgnoringCase(header, friendsPopupScript.TextHeader.text);

		Assert.IsTrue(friendsPopupScript.TabFriendsReferenceToggle.isOn);

		switch (friendsPopupScript.Store.State.FriendsReference.Status) {
			case FriendsReferenceStatus.Active:
				friendsPopupScript.FriendsReferenceActive.SetActive(true);
				Assert.IsTrue(friendsPopupScript.FriendsReferenceActive.activeInHierarchy);
				break;
			case FriendsReferenceStatus.Inactive:
				friendsPopupScript.FriendsReferenceInactive.SetActive(true);
				Assert.IsTrue(friendsPopupScript.FriendsReferenceInactive.activeInHierarchy);
				break;
			case FriendsReferenceStatus.New:
				friendsPopupScript.FriendsReferenceNew.SetActive(true);
				Assert.IsTrue(friendsPopupScript.FriendsReferenceNew.activeInHierarchy);
				break;
		}

		Assert.AreEqual(status, friendsPopupScript.Store.State.FriendsReference.Status);
		Assert.AreEqual(320f, friendsPopupScript.TextHeader.rectTransform.sizeDelta.x);
		Assert.AreEqual(48.2f, friendsPopupScript.TextHeader.rectTransform.sizeDelta.y);
	}

	[Test]
	public void FriendsPopupTestDispatchOpenFriendsListEditModeTestPasses() {
		friendsPopupScript.Store.Dispatch(new ActionOpenFriendsList() {
			ActionType = ActionType.OpenFriendsList,
			Header = $"เพื่อน 0/100"
		});

		StringAssert.StartsWith("เพื่อน", friendsPopupScript.Store.State.Basic.Header);
		StringAssert.StartsWith("เพื่อน", friendsPopupScript.TextHeader.text);

		Assert.IsTrue(friendsPopupScript.TabFriendsListToggle.isOn);

		Assert.AreEqual(169f, friendsPopupScript.TextHeader.rectTransform.sizeDelta.x);
		Assert.AreEqual(48.2f, friendsPopupScript.TextHeader.rectTransform.sizeDelta.y);
	}

	// [UnityTest]
	// public IEnumerator FriendPopupEditModeTestWithEnumeratorPasses() {
	// 	yield return null;
	// }
}
