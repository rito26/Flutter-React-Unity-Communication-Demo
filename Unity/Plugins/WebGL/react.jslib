mergeInto(LibraryManager.library, {
  UnityToReact: function (action, data) {
    window.dispatchReactUnityEvent("UnityToReact", UTF8ToString(action), UTF8ToString(data));
  },
});