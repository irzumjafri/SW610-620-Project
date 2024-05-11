**VR Redirected Walking Experiment**

ADD PROJECT POSTER

---

## Overview
This project aims to explore the concept of redirected walking in virtual reality (VR) environments. Redirected walking techniques alter the user's perception of space in VR to enable them to navigate larger virtual environments while physically constrained to a smaller physical space. 

The project compromises of 2 modules:
1. Room Mapping Module: The Room Mapping Module is responsible for creating a virtual space based on the physical environment in which the VR experience takes place. Using input from VR controllers, users can mark the room space excluding any walls, obstacles, and other environmental features that they want to leave out from the physical space. This digital map serves as the foundation for implementing redirected walking techniques by providing spatial constraints and boundaries for the user's movement within the virtual environment.
2. Redirected Walikng Module: The Redirected Walking Module implements various techniques and algorithms to manipulate the user's perception of space in VR, allowing them to navigate larger virtual environments while physically confined to a smaller physical space. By subtly altering the user's movement trajectory using different gains implemented on the camera view, this module creates an artificial movement within the virtual world, even when the user's physical movements are limited. It incorporates techniques such as bending gain, rotation gain, curvature gain, and translation gain to achieve seamless redirection and enhance the user's VR experience.

Here's a demo video of the project:

ADD DEMO VIDEO

---

## Features

1. **Redirected Walking Techniques**: Implement various redirected walking algorithms to explore how they affect the user's perception and navigation in VR.

2. **Virtual Environment Creation**: Develop diverse virtual environments to test redirected walking, including indoor and outdoor scenarios, to assess the effectiveness of the techniques in different settings.

3. **User Interaction**: Allow users to interact with the virtual environment using VR controllers or other input devices to navigate and explore the space.

4. **Data Collection**: Gather data on user behavior, including movement patterns, interaction times, and feedback, to analyze the effectiveness of redirected walking techniques.

5. **Experiment Control**: Provide tools to control experimental parameters such as virtual environment layout, redirected walking algorithms, and user constraints to conduct controlled experiments.

---

## Development Design Decisions

1. **Unity Engine**: Utilize Unity for VR application development due to its robust VR support, extensive documentation, and a wide range of available assets and plugins.

2. **VR Hardware Compatibility**: Ensure compatibility with popular VR hardware such as Oculus Rift, HTC Vive, and Valve Index to reach a broader user base and facilitate wider adoption.

3. **Modular Architecture**: Design the project with a modular architecture developing a physical space mapping module first, and then a redirected walking module to facilitate easy integration of new redirected walking algorithms, virtual environments, and experimental features.

4. **Real-time Feedback**: Implement real-time feedback mechanisms to provide users with visual and cues while walking, enhancing the immersive experience and awareness of the virtual environment.

5. **Firebase Backend**: Implemented Firebase as the backend to log data, as custom backend couldn't be built due to resource constraint, but it would allow transmission of more data at lower costs.

6.  **Ethical Considerations**: Adhere to ethical guidelines for VR research, including obtaining informed consent from participants, ensuring user safety and comfort as using the application for longer sessions may cause diziness or nausea.
---

## Additional Dev Notes

- OpenXR ARanchors couldn't be implemented as they cannot be saved to the Meta Quest headset, and therefore Meta's OVRanchors are used in the Mapping Module to save physical space.
- OpenXR and Meta SDK packages have major conflicts, and installing them both crashes the application, therefore, they can't be used together.
- Meta SDK has been used for passthrough to work, limiting headset support for the application.
- The version of Firebase Firestore being used isn't working with the headset as it relies on Google Play services being installed on the device, and therefore, the headset is unable to share data.
- Logged Data is also logging head movement, but it isn't currenlty being visualized in the web application.
- A Firebase moduly is available within the Unity package Repo, that might free the reliance of Google Play services, but it wasn't tested due to time constraints.
  
---

## Getting Started

To get started with the VR redirected walking experiment project, follow these steps:

1. **Install Unity**: Download and install the Unity game engine from the official website (https://unity.com/).

2. **Clone the Repository**: Clone the project repository from GitHub to your local machine.

4. **Open in Unity**: Open the project in Unity and explore the project structure, including scripts, scenes, and assets.

5. **Install Firebase SDK**: Download the Firebase SDK from [here](https://firebase.google.com/download/unity), and import it as a custom package in Unity.

6. **Run the Experiment**: Build and run the VR application on your preferred VR hardware device (Tested on Meta Quest 2/3) to start experimenting with redirected walking techniques in virtual environments.

7. **Collect and Analyze Data**: Gather data from user interactions and analyze the results from the [Web Application](https://rwe-data.netlify.app/) to gain insights into the effectiveness of redirected walking techniques in VR navigation.

---

## Contributors

- Antti Santala
- Irzum Jafri
- Jannaten Nayem
- Rami Nurmoranta
- Samu Tolijamo
- Tommi Moilanen
- Veeti Salminen

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Acknowledgments

- Special thanks to Tensu and Santeri for supporting us throughout the project.
- Inspired by previous research in the field of redirected walking and VR techniques.
