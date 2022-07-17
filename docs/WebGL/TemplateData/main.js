var canvas = document.querySelector("#unity-canvas");

var config = {
    dataUrl: "Build/WebGL.data",
    frameworkUrl: "Build/WebGL.framework.js",
    codeUrl: "Build/WebGL.wasm",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "Andrew Allbright",
    productName: "gmtk-2022-behavior-trees-take-two",
    productVersion: "0.0.18",
    devicePixelRatio: 1,
}

createUnityInstance(canvas, config);
