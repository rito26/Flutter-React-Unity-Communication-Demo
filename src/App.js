import React, { Fragment, useState, useCallback, useEffect } from "react";
import { Unity, useUnityContext } from "react-unity-webgl";

export default function App() {

  // ================================================================================================================
  // WebGL WASM Profiles
  // ================================================================================================================
  const build_predemo = {
    exp      : ".unityweb",
    dirName  : "WebGL",
    buildName: "PreDemo"
  };

  // React-unity 통신
  const build_comm = {
    exp      : ".unityweb",
    dirName  : "WebGL",
    buildName: "Communication"
  };

  const bd = build_comm;

  // ================================================================================================================
  // Unity Context
  // ================================================================================================================
  const { unityProvider, loadingProgression, isLoaded, addEventListener, removeEventListener, sendMessage } = 
  useUnityContext({
    loaderUrl   : `${bd.dirName}/${bd.buildName}.loader.js`,
    dataUrl     : `${bd.dirName}/${bd.buildName}.data${bd.exp}`,
    frameworkUrl: `${bd.dirName}/${bd.buildName}.framework.js${bd.exp}`,
    codeUrl     : `${bd.dirName}/${bd.buildName}.wasm${bd.exp}`,
    streamingAssetsUrl: `${bd.dirName}/StreamingAssets`,
  });

  // ================================================================================================================
  // Unity To React (To Flutter)
  // ================================================================================================================
  const handleUnityToReact = useCallback((action, data) => {
    console.log(`[React: Unity -> React] Action: ${action}, Data: ${data}`);

    ReactToFlutter(action, data);

  }, []);

  useEffect(() => {
    addEventListener("UnityToReact", handleUnityToReact);
    return () => {
      removeEventListener("UnityToReact", handleUnityToReact);
    };
  }, [addEventListener, removeEventListener, handleUnityToReact]);

  // ================================================================================================================
  // React To Unity
  // ================================================================================================================
  function reactToUnityCall()
  {
                     sendMessage("UnityChannel", "ReactToUnity_MoveUp"  , "123");
    setTimeout(() => sendMessage("UnityChannel", "ReactToUnity_MoveDown", "555"), 1000);
  }

  function reactToUnity(data)
  {
    sendMessage("UnityChannel", "ReactToUnity_MoveUp", `${data}`);
  }

  // ================================================================================================================
  // Flutter To React
  // ================================================================================================================
  useEffect(() => {

    if(isLoaded === true) {
      window.flutterToReact = (data) => {
        console.log(`[React: Flutter -> React] Data: ${data}`);
        reactToUnity(data);
      }
      console.log("* Flutter To React Init *");
    }

    // setTimeout(() => {
    // }, 5000);
    
  }, [isLoaded]);

  // ================================================================================================================
  // React To Flutter
  // ================================================================================================================
  function ReactToFlutter(action, data)
  {
    const jsonData = JSON.stringify({ 
      action: action,
      data: data,
    });
    console.log(`[React: React -> Flutter] ${jsonData}`);
    // console.log(`[React: React -> Flutter] Action: ${action}, Data: ${data}`);
    
    try {
      // window.WebToApp.postMessage(jsonData);

      // eslint-disable-next-line
      WebToApp.postMessage(jsonData);

    } catch(e) {
      // console.log(e);
    }
  }

  // ================================================================================================================
  // Render
  // ================================================================================================================
  return <Fragment>

    <div className="content-header">
      <button 
        onClick={reactToUnityCall}
        >React To Unity
      </button>
    </div>

    <div className="loading-container">
      {!isLoaded && (
        <p className="loading">Loading... {Math.round(loadingProgression * 100)}%</p>
      )}
    </div>

    <div className="UnityOuter">
      <Unity className="UnityInner"
        unityProvider={unityProvider} 
        style={{ visibility: isLoaded ? "visible" : "hidden" }}
      />
    </div>

  </Fragment>
}