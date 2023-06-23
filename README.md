# Flutter-React-Unity 웹뷰 앱 통신 데모
- Flutter: WebView 위젯으로 접속
- React: react-unity-webgl을 통해 유니티 WebGL 캔버스 렌더링
- Unity: WebGL 빌드

# 참고
- <https://github.com/jeffreylanters/react-unity-webgl>
- <https://github.com/jeffreylanters/react-unity-webgl-template>

# 통신 방법

## 1. Unity -> React
- <https://react-unity-webgl.dev/docs/api/event-system>

<br>

- 유니티 - `Assets/Plugins/WebGL/react.jslib` 작성
```jslib
mergeInto(LibraryManager.library, {
  UnityToReact: function (action, data) {
    window.dispatchReactUnityEvent("UnityToReact", UTF8ToString(action), UTF8ToString(data));
  },
});
```

- 유니티 - TODO: 핵심만 요약
```cs

```

- 리액트 - TODO
```js
// TODO
```

<br>

## 2. React -> Unity
- .

<br>

## 3. React -> Flutter
- .

<br>

## 4. Flutter -> React
- .

<br>
