using IWP.Anim;
using UnityEngine;
using UnityEngine.EventSystems;
using static IWP.Anim.AbstractAnim;
using static IWP.Anim.AnimAccessTypes;
using static IWP.General.InitIDs;
using static IWP.General.ListicleDirs;
using static IWP.General.ListicleTypes;
using static IWP.Math.EasingTypes;

namespace IWP.General {
	internal sealed class Listicle2D: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;

		private bool flag;
		private float savedVal;
		private float posOffset;
		private float scaleFactor;
		private GameObject listing;
		private int listingsLen;
		private int listingsLastIndex;
		private int listingsMidIndex;
		private int limit;
		private int midDisplayedIndex;
		private int displacementFromCenterIndex;
		private int i;
		private int modTracker;

		private CanvasGrpFadeAnim fadeAnim;
		private RectTransformTranslateAnim translateAnim;
		private RectTransformScaleAnim scaleAnim;
		private Vector3 pos;

		private GameObject[] containerGOs;
		private int[] displacementsFromCenterIndex;
		private CanvasGroup[] canvasGrps;
		private CanvasGrpFadeAnim[] fadeAnims;
		private RectTransformTranslateAnim[] translateAnims;
		private RectTransformScaleAnim[] scaleAnims;

		[SerializeField]
		private ListicleDir dir;

		[SerializeField]
		private ListicleType type;

		[SerializeField]
		private float unitsApart;

		[SerializeField]
		private GameObject[] listings;

		[SerializeField]
		private int qtyToDisplay;

		[SerializeField]
		private int listingsCenterIndex;

		[SerializeField]
		private string listingsCenterIndexKey;

		[SerializeField]
		private Vector3 startPos;

		[SerializeField]
		private float ptrClickAnimDuration;

		[SerializeField]
		private float ptrClickStartTimeOffset;

		[SerializeField]
		private float scrollAnimDuration;

		[SerializeField]
		private float scrollStartTimeOffset;

		[SerializeField]
		private float unitAnimDuration;

		[SerializeField]
		private float unitStartTimeOffset;

		[SerializeField]
		private EasingType fadeEasingType;

		[SerializeField]
		private EasingType posOffsetEasingType;

		[SerializeField]
		private EasingType scaleEasingType;

		[SerializeField]
		private AnimationCurve posOffsetCurve;

		[SerializeField]
		private AnimationCurve scaleCurve;

		#endregion

		#region Properties

		internal int ListingsCenterIndex {
			get => listingsCenterIndex;
			set => listingsCenterIndex = value;
		}

		#endregion

		#region Ctors and Dtor

		internal Listicle2D(): base() {
			initControl = null;

			flag = false;
			savedVal = 0.0f;
			posOffset = 0.0f;
			scaleFactor = 1.0f;
			listing = null;
			listingsLen = 0;
			listingsLastIndex = 0;
			listingsMidIndex = 0;
			limit = 0;
			midDisplayedIndex = 0;
			displacementFromCenterIndex = 0;
			i = 0;
			modTracker = 0;

			fadeAnim = null;
			translateAnim = null;
			scaleAnim = null;
			pos = Vector3.zero;

			containerGOs = System.Array.Empty<GameObject>();
			displacementsFromCenterIndex = System.Array.Empty<int>();
			canvasGrps = System.Array.Empty<CanvasGroup>();
			fadeAnims = System.Array.Empty<CanvasGrpFadeAnim>();
			translateAnims = System.Array.Empty<RectTransformTranslateAnim>();
			scaleAnims = System.Array.Empty<RectTransformScaleAnim>();

			dir = ListicleDir.Amt;
			type = ListicleType.Amt;

			unitsApart = 0.0f;
			listings = System.Array.Empty<GameObject>();
			qtyToDisplay = 0;
			listingsCenterIndex = 0;
			listingsCenterIndexKey = string.Empty;
			startPos = Vector3.zero;

			ptrClickAnimDuration = 0.0f;
			ptrClickStartTimeOffset = 0.0f;
			scrollAnimDuration = 0.0f;
			scrollStartTimeOffset = 0.0f;
			unitAnimDuration = 0.0f;
			unitStartTimeOffset = 0.0f;
			fadeEasingType = EasingType.Amt;
			posOffsetEasingType = EasingType.Amt;
			scaleEasingType = EasingType.Amt;

			posOffsetCurve = null;
			scaleCurve = null;
		}

		static Listicle2D() {
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void OnValidate() {
			UnityEngine.Assertions.Assert.AreNotEqual(dir, ListicleDir.Amt, "dir, ListicleDir.Amt");
			UnityEngine.Assertions.Assert.AreNotEqual(type, ListicleType.Amt, "type, ListicleType.Amt");

			UnityEngine.Assertions.Assert.IsTrue(listings.Length > 1, "listings.Length > 1");
			UnityEngine.Assertions.Assert.IsTrue((qtyToDisplay & 1) == 1, "(qtyToDisplay & 1) == 1");
			UnityEngine.Assertions.Assert.IsTrue((listings.Length & 1) == 1, "(listings.Length & 1) == 1");
			UnityEngine.Assertions.Assert.IsTrue(
				qtyToDisplay <= listings.Length,
				"qtyToDisplay <= listings.Length"
			);
			UnityEngine.Assertions.Assert.IsTrue(
				listingsCenterIndex < listings.Length,
				"listingsCenterIndex < listings.Length"
			);

			UnityEngine.Assertions.Assert.AreNotEqual(
				fadeEasingType, EasingType.Amt,
				"fadeEasingType, EasingType.Amt"
			);
			UnityEngine.Assertions.Assert.AreNotEqual(
				posOffsetEasingType, EasingType.Amt,
				"posOffsetEasingType, EasingType.Amt"
			);
			UnityEngine.Assertions.Assert.AreNotEqual(
				scaleEasingType, EasingType.Amt,
				"scaleEasingType, EasingType.Amt"
			);

			UnityEngine.Assertions.Assert.IsTrue(posOffsetCurve.length > 0, "posOffsetCurve.length > 0");
			UnityEngine.Assertions.Assert.IsTrue(scaleCurve.length > 0, "scaleCurve.length > 0");
		}

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.Listicle2D, Init);
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.Listicle2D, Init);
		}

		#endregion

		private void Init() {
			listingsLen = listings.Length;
			listingsLastIndex = listingsLen - 1;
			listingsMidIndex = listingsLastIndex / 2;
			limit = (listingsLen + 1) / 2;
			midDisplayedIndex = (qtyToDisplay - 1) / 2;

			if(!string.IsNullOrEmpty(listingsCenterIndexKey)) {
				listingsCenterIndex = PlayerPrefs.GetInt(listingsCenterIndexKey, 0);
			}

			if(type == ListicleType.Regular) {
				limit += listingsLen - 1;
				modTracker = listingsMidIndex - listingsCenterIndex;
			}

			int qtyToAffect;
			Vector3 scale = Vector3.one;

			containerGOs = new GameObject[limit];
			displacementsFromCenterIndex = new int[listingsLen];
			canvasGrps = new CanvasGroup[listingsLen];
			fadeAnims = new CanvasGrpFadeAnim[listingsLen];
			translateAnims = new RectTransformTranslateAnim[listingsLen];
			scaleAnims = new RectTransformScaleAnim[listingsLen];

			GameObject containerGO;
			for(i = 0; i < limit; ++i) {
				containerGO = new GameObject("Container" + i.ToString());
				containerGO.transform.SetParent(transform, false);
				containerGOs[i] = containerGO;
			}

			CanvasGroup canvasGrp;
			EventTrigger eventTrigger;
			EventTrigger.Entry ptrClickEntry;

			EventTrigger.Entry scrollEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.Scroll
			};
			scrollEntry.callback.AddListener((eventData) => {
				PointerEventData ptrEventData = (PointerEventData)eventData;

				if(ptrEventData.scrollDelta.y > 0.0f) {
					_ = Progress(scrollAnimDuration, scrollStartTimeOffset, -1);
				}

				if(ptrEventData.scrollDelta.y < 0.0f) {
					_ = Progress(scrollAnimDuration, scrollStartTimeOffset, 1);
				}
			});

			EventTrigger.Entry initializePotentialDragEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.InitializePotentialDrag
			};
			initializePotentialDragEntry.callback.AddListener((eventData) => {
				OnInitializePotentialDragHandler((PointerEventData)eventData);
			});

			EventTrigger.Entry endDragEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.EndDrag
			};
			endDragEntry.callback.AddListener((eventData) => {
				OnEndDragHandler((PointerEventData)eventData);
			});

			for(i = 0; i < listingsLen; ++i) {
				pos = startPos;
				listing = listings[i];

				canvasGrp = listing.AddComponent<CanvasGroup>();
				canvasGrps[i] = canvasGrp;

				int indexCopy = i;
				eventTrigger = listing.AddComponent<EventTrigger>();
				eventTrigger.triggers.Add(scrollEntry);
				eventTrigger.triggers.Add(initializePotentialDragEntry);
				eventTrigger.triggers.Add(endDragEntry);

				ptrClickEntry = new EventTrigger.Entry {
					eventID = EventTriggerType.PointerClick
				};
				ptrClickEntry.callback.AddListener((eventData) => {
					_ = StartCoroutine(OnPtrClickFunc(displacementsFromCenterIndex[indexCopy]));
				});
				eventTrigger.triggers.Add(ptrClickEntry);

				fadeAnim = listing.AddComponent<CanvasGrpFadeAnim>();
				fadeAnim.AccessType = AnimAccessType.Script;

				//fadeAnim.periodicDelay = 0.0f;
				fadeAnim.countThreshold = 1;
				fadeAnim.canvasGrp = canvasGrp;
				fadeAnim.easingType = fadeEasingType;

				fadeAnim.InitMe();
				fadeAnims[i] = fadeAnim;

				translateAnim = listing.AddComponent<RectTransformTranslateAnim>();
				scaleAnim = listing.AddComponent<RectTransformScaleAnim>();
				translateAnim.AccessType = scaleAnim.AccessType = AnimAccessType.Script;

				//translateAnim.periodicDelay = scaleAnim.periodicDelay = 0.0f;
				translateAnim.countThreshold = scaleAnim.countThreshold = 1;
				translateAnim.myRectTransform = scaleAnim.myRectTransform = (RectTransform)listing.transform;
				translateAnim.easingType = posOffsetEasingType;
				scaleAnim.easingType = scaleEasingType;

				translateAnim.InitMe();
				translateAnims[i] = translateAnim;

				scaleAnim.InitMe();
				scaleAnims[i] = scaleAnim;

				if(type == ListicleType.Regular) {
					displacementsFromCenterIndex[i] = i - listingsCenterIndex;
				} else {
					if(i == listingsCenterIndex) {
						displacementsFromCenterIndex[i] = 0;
					} else if(listingsCenterIndex == listingsMidIndex) {
						displacementsFromCenterIndex[i] = i - listingsCenterIndex;
					} else {
						qtyToAffect = Mathf.Abs(listingsCenterIndex - listingsMidIndex);

						if(listingsCenterIndex > listingsMidIndex) {
							displacementsFromCenterIndex[i] = (i < qtyToAffect ? i + listingsLastIndex + 1 : i) - listingsCenterIndex;
						} else {
							displacementsFromCenterIndex[i] = (i > listingsLastIndex - qtyToAffect ? i - (listingsLastIndex + 1) : i) - listingsCenterIndex;
						}
					}
				}

				displacementFromCenterIndex = displacementsFromCenterIndex[i];

				if(Mathf.Abs(displacementFromCenterIndex) > midDisplayedIndex) {
					canvasGrp.alpha = 0.0f;
				}

				listing.transform.SetParent(containerGOs[limit - 1 - Mathf.Abs(displacementFromCenterIndex)].transform, false);

				if(type == ListicleType.Circular && Mathf.Abs(displacementFromCenterIndex) > midDisplayedIndex + 1) {
					continue;
				}

				CalcPosOffsetAndScaleFactor();

				SpaceOut();

				if(dir == ListicleDir.Horiz) {
					pos.y += posOffset;
				} else {
					pos.x += posOffset;
				}
				listing.transform.localPosition = pos;

				scale.x = scale.y = scaleFactor;
				listing.transform.localScale = scale;
			}
		}

		public void ProgressForwardUnit() {
			_ = Progress(unitAnimDuration, unitStartTimeOffset, 1);
		}

		public void ProgressBackwardUnit() {
			_ = Progress(unitAnimDuration, unitStartTimeOffset, -1);
		}

		private bool Progress(float animDuration, float startTimeOffset, int x) {
			for(i = 0; i < listingsLen; ++i) {
				if(translateAnims[i].IsUpdating) {
					return false;
				}
			}

			if(type == ListicleType.Regular) {
				if(x > 0) {
					if(--modTracker >= -listingsMidIndex) {
						flag = false;
					} else {
						modTracker = -listingsMidIndex;
						return false;
					}
				} else {
					if(++modTracker <= listingsMidIndex) {
						flag = true;
					} else {
						modTracker = listingsMidIndex;
						return false;
					}
				}
			}

			for(i = 0; i < listingsLen; ++i) {
				displacementFromCenterIndex = displacementsFromCenterIndex[i];

				if(type == ListicleType.Circular && Mathf.Abs(displacementFromCenterIndex) > midDisplayedIndex + 1) {
					continue;
				}

				if(type == ListicleType.Circular) {
					if(x > 0) {
						if(--displacementFromCenterIndex < -listingsMidIndex) {
							displacementFromCenterIndex = listingsMidIndex;
						}
					} else if(++displacementFromCenterIndex > listingsMidIndex) {
						displacementFromCenterIndex = -listingsMidIndex;
					}
				} else {
					if(flag) {
						_ = ++displacementFromCenterIndex;
					} else {
						_ = --displacementFromCenterIndex;
					}
				}

				pos = startPos;
				listing = listings[i];

				fadeAnim = fadeAnims[i];
				translateAnim = translateAnims[i];
				scaleAnim = scaleAnims[i];

				fadeAnim.animDuration = animDuration * 0.5f;
				translateAnim.animDuration = scaleAnim.animDuration = animDuration;
				fadeAnim.startTimeOffset = translateAnim.startTimeOffset = scaleAnim.startTimeOffset = startTimeOffset;

				translateAnim.startPos = listing.transform.localPosition;
				scaleAnim.startScale = listing.transform.localScale;

				if(displacementFromCenterIndex == (midDisplayedIndex + 1) * -x) {
					fadeAnim.startAlpha = 1.0f;
					fadeAnim.endAlpha = 0.0f;
					fadeAnim.IsUpdating = true;
				}

				CalcPosOffsetAndScaleFactor();

				SpaceOut();

				if(dir == ListicleDir.Horiz) {
					pos.y += posOffset;
				} else {
					pos.x += posOffset;
				}

				if(displacementFromCenterIndex == listingsMidIndex * x) {
					if(dir == ListicleDir.Horiz) {
						translateAnim.endPos = pos;

						translateAnim.endPos.x = listing.transform.localPosition.x
							- (unitsApart + ((RectTransform)listing.transform).rect.width * scaleFactor) * x;
					} else {
						translateAnim.endPos = pos;

						translateAnim.endPos.y = listing.transform.localPosition.y
							- (unitsApart + ((RectTransform)listing.transform).rect.width * scaleFactor) * x;
					}
				} else {
					translateAnim.endPos = pos;
				}

				AnimDelegate myDelegate = null;
				int indexCopy = i;
				int displacementFromCenterIndexCopy = displacementFromCenterIndex;
				int midDisplayedIndexCopy = midDisplayedIndex;

				if(type == ListicleType.Circular) {
					float scaleFactorCopy = scaleFactor;
					int listingsMidIndexCopy = listingsMidIndex;

					myDelegate += () => {
						listing = listings[indexCopy];
						translateAnim = translateAnims[indexCopy];

						if(displacementFromCenterIndexCopy == midDisplayedIndexCopy * x) {
							if(qtyToDisplay == listingsLen) {
								AnimDelegate myEndDelegate = null;
								fadeAnim = fadeAnims[indexCopy];

								myEndDelegate += () => {
									fadeAnim.startAlpha = 0.0f;
									fadeAnim.endAlpha = 1.0f;
									fadeAnim.IsUpdating = true;

									fadeAnim.animEndDelegate -= myEndDelegate;
								};

								fadeAnim.animEndDelegate += myEndDelegate;
							} else {
								fadeAnim = fadeAnims[indexCopy];
								fadeAnim.startAlpha = 0.0f;
								fadeAnim.endAlpha = 1.0f;
								fadeAnim.IsUpdating = true;
							}
						}

						if(displacementFromCenterIndexCopy == listingsMidIndexCopy * x) {
							int myIndex;
							float myScaleFactor;
							float offset = 0.0f;
							int absVal = Mathf.Abs(displacementFromCenterIndexCopy);

							for(int j = -absVal; j <= absVal; ++j) {
								myIndex = listingsCenterIndex + j;

								if(myIndex > listingsLen - 1) {
									myIndex -= listingsLen;
								} else if(myIndex < 0) {
									myIndex += listingsLen;
								}

								if(qtyToDisplay == listingsLen) {
									myScaleFactor = scaleCurve.Evaluate(
										scaleCurve[scaleCurve.length - 1].time //Latest time
										/ (qtyToDisplay - 1)
										* (j + midDisplayedIndexCopy)
									);
								} else {
									myScaleFactor = scaleCurve.Evaluate(
										scaleCurve[scaleCurve.length - 1].time //Latest time
										/ (qtyToDisplay + 2 - 1)
										* (j + midDisplayedIndexCopy + 1)
									);
								}

								offset += (((RectTransform)listings[myIndex].transform).rect.width * myScaleFactor + unitsApart) * x;
							}

							if(dir == ListicleDir.Horiz) {
								translateAnim.startPos.x += offset;
								translateAnim.endPos.x += offset;
							} else {
								translateAnim.startPos.y += offset;
								translateAnim.endPos.y += offset;
							}
						}

						listing.transform.SetParent(containerGOs[limit - 1 - Mathf.Abs(displacementFromCenterIndexCopy)].transform, false);

						translateAnim.animPreMidDelegate -= myDelegate;
					};
				} else {
					myDelegate += () => {
						listing = listings[indexCopy];
						translateAnim = translateAnims[indexCopy];

						if((x > 0 && displacementFromCenterIndexCopy == midDisplayedIndexCopy) || (x < 0 && displacementFromCenterIndexCopy == -midDisplayedIndexCopy)) {
							fadeAnim = fadeAnims[indexCopy];
							fadeAnim.startAlpha = 0.0f;
							fadeAnim.endAlpha = 1.0f;
							fadeAnim.IsUpdating = true;
						}

						listing.transform.SetParent(containerGOs[limit - 1 - Mathf.Abs(displacementFromCenterIndexCopy)].transform, false);

						translateAnim.animPreMidDelegate -= myDelegate;
					};
				}

				translateAnim.animPreMidDelegate += myDelegate;
				translateAnim.IsUpdating = true;

				scaleAnim.endScale.x = scaleAnim.endScale.y = scaleFactor;
				scaleAnim.IsUpdating = true;

				displacementsFromCenterIndex[i] = displacementFromCenterIndex;
			}

			return true;
		}

		private void CalcPosOffsetAndScaleFactor() {
			if(qtyToDisplay == listingsLen) {
				posOffset = posOffsetCurve.Evaluate(
					posOffsetCurve[posOffsetCurve.length - 1].time //Latest time
					/ (qtyToDisplay - 1)
					* (displacementFromCenterIndex + midDisplayedIndex)
				);

				scaleFactor = scaleCurve.Evaluate(
					scaleCurve[scaleCurve.length - 1].time //Latest time
					/ (qtyToDisplay - 1)
					* (displacementFromCenterIndex + midDisplayedIndex)
				);
			} else {
				posOffset = posOffsetCurve.Evaluate(
					posOffsetCurve[posOffsetCurve.length - 1].time //Latest time
					/ (qtyToDisplay + 2 - 1)
					* (displacementFromCenterIndex + midDisplayedIndex + 1)
				);

				scaleFactor = scaleCurve.Evaluate(
					scaleCurve[scaleCurve.length - 1].time //Latest time
					/ (qtyToDisplay + 2 - 1)
					* (displacementFromCenterIndex + midDisplayedIndex + 1)
				);
			}
		}

		private void SpaceOut() {
			if(displacementFromCenterIndex == 0) {
				return;
			}

			int myIndex;
			float myScaleFactor;
			float val;

			if(displacementFromCenterIndex > 0) {
				for(int j = 0; j < displacementFromCenterIndex; ++j) {
					myIndex = listingsCenterIndex + j;

					if(myIndex > listingsLen - 1) {
						myIndex -= listingsLen;
					}

					if(qtyToDisplay == listingsLen) {
						myScaleFactor = scaleCurve.Evaluate(
							scaleCurve[scaleCurve.length - 1].time //Latest time
							/ (qtyToDisplay - 1)
							* (j + midDisplayedIndex)
						);
					} else {
						myScaleFactor = scaleCurve.Evaluate(
							scaleCurve[scaleCurve.length - 1].time //Latest time
							/ (qtyToDisplay + 2 - 1)
							* (j + midDisplayedIndex + 1)
						);
					}

					val = ((RectTransform)listings[myIndex].transform).rect.width * myScaleFactor * (myIndex == listingsCenterIndex ? 0.5f : 1.0f) + unitsApart;

					if(dir == ListicleDir.Horiz) {
						pos.x += val;
					} else {
						pos.y += val;
					}
				}

				val = ((RectTransform)listing.transform).rect.width * scaleFactor * 0.5f;

				if(dir == ListicleDir.Horiz) {
					pos.x += val;
				} else {
					pos.y += val;
				}
			} else {
				for(int j = displacementFromCenterIndex + 1; j <= 0; ++j) {
					myIndex = listingsCenterIndex + j;

					if(myIndex < 0) {
						myIndex += listingsLen;
					}

					if(qtyToDisplay == listingsLen) {
						myScaleFactor = scaleCurve.Evaluate(
							scaleCurve[scaleCurve.length - 1].time //Latest time
							/ (qtyToDisplay - 1)
							* (j + midDisplayedIndex)
						);
					} else {
						myScaleFactor = scaleCurve.Evaluate(
							scaleCurve[scaleCurve.length - 1].time //Latest time
							/ (qtyToDisplay + 2 - 1)
							* (j + midDisplayedIndex + 1)
						);
					}

					val = ((RectTransform)listings[myIndex].transform).rect.width * myScaleFactor * (myIndex == listingsCenterIndex ? 0.5f : 1.0f) + unitsApart;

					if(dir == ListicleDir.Horiz) {
						pos.x -= val;
					} else {
						pos.y -= val;
					}
				}

				val = ((RectTransform)listing.transform).rect.width * scaleFactor * 0.5f;

				if(dir == ListicleDir.Horiz) {
					pos.x -= val;
				} else {
					pos.y -= val;
				}
			}
		}

		private System.Collections.IEnumerator OnPtrClickFunc(int displacementFromCenterIndexParam) {
			int absDisplacementFromCenterIndex = Mathf.Abs(displacementFromCenterIndexParam);

			while(absDisplacementFromCenterIndex > 0) {
				if(displacementFromCenterIndexParam > 0) {
					if(Progress(ptrClickAnimDuration, ptrClickStartTimeOffset, 1)) {
						_ = --absDisplacementFromCenterIndex;
					}
				} else {
					if(Progress(ptrClickAnimDuration, ptrClickStartTimeOffset, -1)) {
						_ = --absDisplacementFromCenterIndex;
					}
				}

				yield return null;
			}
		}

		#pragma warning disable IDE0060 //Remove unused parameter

		private void OnInitializePotentialDragHandler(PointerEventData ptrEventData) {
			savedVal = dir == ListicleDir.Horiz ? Input.mousePosition.x : Input.mousePosition.y;
		}

		private void OnEndDragHandler(PointerEventData ptrEventData) {
			_ = Progress(
				ptrClickAnimDuration,
				ptrClickStartTimeOffset,
				(dir == ListicleDir.Horiz ? Input.mousePosition.x : Input.mousePosition.y) - savedVal > 0.0f ? -1 : 1
			);
		}

		#pragma warning restore IDE0060 //Remove unused parameter
	}
}