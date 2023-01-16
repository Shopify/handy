# handy

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/logo.PNG" width="600"/>
 <p align="center">
  <em>The easiest way to mocap your hands!</em>
 </p>
</p>

## Background

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/animated_hands.gif" width="600"/>
 <p align="center">
  <em>No artists had to suffer to make this Blender animation. It was all mocapped using a Meta Quest Pro!</em>
 </p>
</p>

We developed this tool in order to streamline the process of capturing hand movements from Meta Quest headsets and bringing them into Blender for use in animations.

## Examples of what you can do with handy

- The hands in [this](https://twitter.com/StrangeNative/status/1613218237969494017?s=20) concept video were mocapped using handy!

<p align="center">
  <a href="">
    <img src="https://github.com/Shopify/handy/blob/main/readme_images/concept_video.PNG" href="https://twitter.com/StrangeNative/status/1613218237969494017?s=20">
  </a>
</p>

- The hands and face in [this](https://diegomacario.github.io/Hands-In-The-Web) live demo were also mocapped using handy!

<p align="center">
  <a href="">
    <img src="https://github.com/Shopify/handy/blob/main/readme_images/geisha.png" href="https://diegomacario.github.io/Hands-In-The-Web">
  </a>
</p>

The code behind that live demo is available [here](https://github.com/diegomacario/Hands-In-The-Web), in case you are interested in how mocap data can be played in the browser.

## Prerequisites

* Unity 2022.1.23 or later
* Meta Quest (1, 2, or Pro)
* Windows or macOS

If you've never built and installed a Unity project on a Meta Quest headset, start by reading the [beginner's guide](https://github.com/Shopify/handy/blob/main/BEGINNERS_GUIDE.md). It will walk you through everything you need to do!

We wrote it so that people that don't have a technical background can run this project too.

## Workflows

We developed two workflows for this project. The `Optimized Workflow` is fast, but it requires an internet connection so that the `Handy` app can send mocap recordings from the headset to the Unity editor to automatically generate Alembic files. The `Manual Workflow` is slow, but it doesn't require an internet connection. 

### Optimized Workflow

<details>
  <summary>Click to expand</summary>

1. First, build the `ClientScene` and install it to the headset. You can find it here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/client_scene.png" width="600"/>
</p>

To build and install it you can simply go to `File -> Build And Run`.

2. Now, switch over to the `ServerScene` - double-click it to open it, then hit play in the editor. You can find the scene here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/server_scene.png" width="600"/>
</p>

Play in the editor looks like this:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/editor_play_button.png" width="600"/>
</p>

3. Run the `Handy` app on the headset.

4. Start and stop recording by pinching your left thumb and index finger together and holding the pinch until the red recording indicator appears or disappears at your left wrist.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/begin_and_end_recording.gif" width="600"/>
 <p align="center">
  <em>The red sphere at the left wrist indicates whether you are recording or not.</em>
 </p>
</p>

5. Every time you stop recording, the client (the `Handy` app) will send a `.jsonlines` file to the server (the Unity editor). The server will then immediately start playing back the recording (you will see your hands moving in Unity's viewport). Once it finishes playing the recording (you will see your hands freeze in Unity's viewport), it will output a finished `.abc` filename in Unity's console.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/final_abc.png" width="600"/>
</p>

The `.abc` files will always be placed in a folder called `data_output` which is located at the root of your clone of this repository.

6. If you start and stop recording multiple times in a row, a queue will form in the server, so you will have to wait for it to process all your recordings.

7. Load your exported `.abc` files in Blender. You will see two hands and a cube that acts as a placeholder for the headset, which we also record!

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/hands_and_head.PNG" width="600"/>
</p>
</details>

### Manual Workflow

<details>
  <summary>Click to expand</summary>

1. First, build the `CaptureScene` and install it to the headset. You can find it here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/capture_scene.PNG" width="600"/>
</p>

Double-click it to open it, and then go to `File -> Build Settings...`, select any scenes that are in the `Scenes In Build` box, right click them and select `Remove Selection`.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/remove_selection.png" width="600"/>
</p>

After that simply click the `Add Open Scenes` button and the `CaptureScene` should be added to the list. You are now ready to press the `Build And Run` button.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/add_open_scenes.png" width="600"/>
</p>

2. Run the `Handy` app on the headset.
3. Start and stop recording by pinching your left thumb and index finger together and holding the pinch until the red recording indicator appears or disappears at your left wrist.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/begin_and_end_recording.gif" width="600"/>
 <p align="center">
  <em>The red sphere at the left wrist indicates whether you are recording or not.</em>
 </p>
</p>

4. Connect the headset to your computer and download the `.jsonlines` files that were recorded. You can find them here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/jsonlines_folder.PNG" width="600"/>
</p>

5. Open the `PlaybackScene` in the Unity editor by double-clicking it. You can find it here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/playback_scene.PNG" width="600"/>
</p>

6. Click on the `PlaybackManager` object in the scene hierarchy. In the `PlaybackManager` component of that object, input the path of the `.jsonlines` file that you want to export as an Alembic file. In the `AlembicExporter` component of the same object, specify the location where you want the Alembic file to be generated and its name.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/playback_steps.PNG" width="600"/>
</p>

7. Hit play in the editor and wait for the animation to complete. Play in the editor looks like this:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/editor_play_button.png" width="600"/>
</p>

8. Load your exported `.abc` file in Blender. You will see two hands and a cube that acts as a placeholder for the headset, which we also record!

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/hands_and_head.PNG" width="600"/>
</p>
</details>

## License

This project is licensed under the MIT License - see the LICENSE file for details.