// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
// import { getAnalytics } from "firebase/analytics";
import { getFirestore } from "firebase/firestore";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyB_Fm7eRd9sS2wWCpwO1d_b74KiMIPdYLU",
  authDomain: "redirected-walking-experiment.firebaseapp.com",
  databaseURL:
    "https://redirected-walking-experiment-default-rtdb.europe-west1.firebasedatabase.app",
  projectId: "redirected-walking-experiment",
  storageBucket: "redirected-walking-experiment.appspot.com",
  messagingSenderId: "357552106369",
  appId: "1:357552106369:web:f1460d9d3929a944911680",
  measurementId: "G-4PD0YEC007",
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
// const analytics = getAnalytics(app);
const db = getFirestore(app);

export { db };
