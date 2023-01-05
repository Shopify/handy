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

We developed this tool in order to streamline the process of capturing hand movements from VR devices and bringing them into Blender for use in animations.


## Optimized Workflow

1. First, build `ClientScene` and install it to the VR device. You can find it here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/client_scene.png" width="600"/>
</p>

2. Now, switch over to `ServerScene` - double-click it to open it, then hit play in the editor. You can find the scene here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/server_scene.png" width="600"/>
</p>

Play in the editor looks like this:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/editor_play_button.png" width="600"/>
</p>

3. Run the `Handy` app on the VR device.

4. Start and stop recording by pinching your left thumb and index finger together and holding the pinch until the red recording indicator appears or disappears at your left wrist.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/begin_and_end_recording.gif" width="600"/>
 <p align="center">
  <em>The red sphere at the left wrist indicates whether you are recording or not.</em>
 </p>
</p>

5. Watch the editor and wait for the animation to complete and output a finished `.abc` filename.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/final_abc.png" width="848"/>
</p>

6. Load your exported `.abc` files in Blender. You will see the two hands and a cube that acts as a placeholder for the headset, which we also record!

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/hands_and_head.PNG" width="600"/>
</p>

## Workflow

1. First, build the `CaptureScene` and install it to the VR device. You can find it here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/capture_scene.PNG" width="600"/>
</p>

2. Run the `Handy` app on the VR device.
3. Start and stop recording by pinching your left thumb and index finger together and holding the pinch until the red recording indicator appears or disappears at your left wrist.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/begin_and_end_recording.gif" width="600"/>
 <p align="center">
  <em>The red sphere at the left wrist indicates whether you are recording or not.</em>
 </p>
</p>

4. Connect the VR device to your computer and download the `.jsonlines` files that were recorded. You can find them here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/jsonlines_folder.PNG" width="600"/>
</p>

5. Open the `PlaybackScene` in the Unity editor. You can find it here:

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/playback_scene.PNG" width="600"/>
</p>

6. Click on the `PlaybackManager` object in the scene hierarchy. In the `PlaybackManager` component of that object, input the path of the `.jsonlines` file that you want to export as an Alembic file. In the `AlembicExporter` component of the same object, specify the location where you want the Alembic file to be generated and its name.

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/playback_steps.PNG" width="600"/>
</p>

7. Hit play in the editor and wait for the animation to complete.
8. Load your exported `.abc` file in Blender. You will see the two hands and a cube that acts as a placeholder for the headset, which we also record!

<p align="center">
 <img src="https://github.com/Shopify/handy/blob/main/readme_images/hands_and_head.PNG" width="600"/>
</p>

## Prerequisites
* Unity 2022.1.23 or later
* Meta Quest (1, 2, or Pro)


## License

This project is licensed under the MIT License - see the LICENSE file for details.