// <copyright file="ObjectSelectState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// State when selecting a building that you want to place.
    /// </summary>
    public class ObjectSelectState : AbstractState
    {
        /// <summary>
        /// Amount of columns allowed in the frame for object selecting
        /// </summary>
        private const int ColumnCount = 3;

        /// <summary>
        /// The width of an entry for the object selecting
        /// </summary>
        private const int EntryWidth = 160;

        /// <summary>
        /// The height of an entry for the object selecting
        /// </summary>
        private const int EntryHeight = 160;

        /// <summary>
        /// Amount of padding for an entry
        /// </summary>
        private const int Padding = 10;

        /// <summary>
        /// Width of the select frame displayed
        /// </summary>
        private const int FrameWidth = 270;

        /// <summary>
        /// The location at which the objects are stored.
        /// </summary>
        private const string ObjectPath = "Objects";

        /// <summary>
        /// Path to the preview location
        /// </summary>
        private const string PreviewPath = "Objects/Previews/";

        /// <summary>
        /// The scroll pane with the objects that can be selected.
        /// </summary>
        private GameObject objectSelect;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSelectState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public ObjectSelectState(StateContext stateContext) : base(stateContext)
        {
            this.objectSelect = this.InitObjectSelect(ColumnCount, EntryWidth, EntryHeight, Padding, FrameWidth);
            this.InitButton("ToggleButton", () => this.OnToggleButton());
        }

        /// <summary>
        /// Disable the dropdown and go back to the neutral state
        /// </summary>
        public void OnToggleButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Destroy the object select
        /// </summary>
        public override void DiscardState()
        {
            base.DiscardState();
            UnityEngine.Object.Destroy(this.objectSelect);
        }

        /// <summary>
        /// Set the current selected object to a new one
        /// </summary>
        /// <param name="resourcePath">The path of the object located in Resources/Objects/</param>
        public void SelectObjectButtonEvent(string resourcePath)
        {
            this.StateContext.SwapObject("Objects/" + resourcePath);
        }

        /// <summary>
        /// Initialized the object select frame
        /// </summary>
        /// <param name="columnSize">amount of columns for entries</param>
        /// <param name="entryWidth">the width of an entry</param>
        /// <param name="entryHeight">the height of an entry</param>
        /// <param name="padding">the padding of an entry to either sides</param>
        /// <param name="frameWidth">the preferred size of the frame</param>
        /// <returns>The Object select frame game object</returns>
        private GameObject InitObjectSelect(int columnSize, int entryWidth, int entryHeight, int padding, int frameWidth)
        {
            GameObject objectSelect = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/ObjectSelect"));
            GameObject[] objects = Resources.LoadAll<GameObject>(ObjectPath);
            GameObject scrollViewVertical = objectSelect.transform.GetChild(0).GetChild(1).gameObject;
            this.SetRectTransform(scrollViewVertical.GetComponent<RectTransform>(), new Vector3(-80, -35, 0), new Vector2((entryWidth + (2 * padding)) * columnSize, frameWidth));
            GameObject content = scrollViewVertical.transform.GetChild(0).gameObject;
            this.SetRectTransform(
                content.GetComponent<RectTransform>(),
                new Vector3(-frameWidth, 135 - ((entryHeight + (2 * padding)) * (1 + Mathf.Floor(objects.Length / columnSize)))),
                new Vector2((entryWidth + (2 * padding)) * columnSize, (entryHeight + (2 * padding)) * (1 + Mathf.Floor(objects.Length / columnSize))));
            float deductY = content.GetComponent<RectTransform>().sizeDelta.y;
            GameObject scrollViewEntry = content.transform.GetChild(0).gameObject;
            for (int i = 0; i < objects.Length; i++)
            {
                this.AddScrollEntry(GameObject.Instantiate(scrollViewEntry), objects[i].name, deductY, content.transform, i, columnSize, entryWidth, entryHeight, padding);
            }

            UnityEngine.Object.Destroy(scrollViewEntry);
            objectSelect.transform.SetParent(ButtonWrapper.Wrapper, false);
            return objectSelect;
        }

        /// <summary>
        /// Edit a rectangle transform
        /// </summary>
        /// <param name="transform">the rectangle transform to modify</param>
        /// <param name="localPosition">the new local position</param>
        /// <param name="sizeDelta">the new size delta</param>
        private void SetRectTransform(RectTransform transform, Vector3 localPosition, Vector2 sizeDelta)
        {
            transform.localPosition = localPosition;
            transform.sizeDelta = sizeDelta;
        }

        /// <summary>
        /// Initialize a scroll entry
        /// </summary>
        /// <param name="entry">The entry game object</param>
        /// <param name="name">name of the object</param>
        /// <param name="deductY">the correction of the local position y of the parent</param>
        /// <param name="parent">the transform to set as parent, should be content</param>
        /// <param name="i">the increment counter to place the entry at the correct row and column</param>
        /// <param name="columnSize">the amount of columns</param>
        /// <param name="entryWidth">width of an entry</param>
        /// <param name="entryHeight">height of an entry</param>
        /// <param name="padding">padding to either sides of the entry</param>
        private void AddScrollEntry(
            GameObject entry, string name, float deductY, Transform parent, int i, int columnSize, int entryWidth, int entryHeight, int padding)
        {
            Transform image = entry.transform.GetChild(0);
            image.GetComponent<Image>().sprite = this.CreateImage(name);
            entry.transform.SetParent(parent);
            entry.name = name;
            entry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            entry.GetComponent<RectTransform>().localPosition = new Vector3(
                (entryWidth / 2) + padding + (Mathf.Floor(i % columnSize) * (entryWidth + (2 * padding))),
                (entryWidth / 2) - ((Mathf.Floor((i / columnSize) + 1) * entryHeight) + (2 * padding) - deductY));
            entry.transform.GetComponentInChildren<Text>().text = name;
            entry.AddComponent<Selector>().Init(this);
            if (name == "DefaultObject")
            {
                entry.GetComponent<Selector>().OnPointerDown(null);
            }
        }

        /// <summary>
        /// Load the sprite of the prefab preview if it exists, return default otherwise
        /// </summary>
        /// <param name="name">name of the prefab</param>
        /// <returns>sprite if it exists</returns>
        private Sprite CreateImage(string name)
        {
            Sprite sprite = Resources.Load<Sprite>(PreviewPath + name);
            if (sprite != null)
            {
                return Resources.Load<Sprite>(PreviewPath + name);
            }
            else
            {
                return Resources.Load<Sprite>(PreviewPath + "DefaultPreview");
            }
        }
    }
}
