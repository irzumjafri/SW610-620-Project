// importing libraries
import { useState, useEffect } from "react";
import { collection, getDocs, doc, orderBy, query } from "firebase/firestore";
// importing components
import Header from "./components/Header";
import Footer from "./components/Footer";
import Loading from "./components/Loading";
import SessionNavigation from "./components/SessionNavigation";
// importing firebase
import { db } from "../firebase";
import "./App.css";

const App = () => {
  const [sessionDataFirebase, setSessionDataFirebase] = useState();
  const [selectedDate, setSelectedDate] = useState();
  const [selectedTestSequence, setSelectedTestSequence] = useState("");
  const [selectedAction, setSelectedAction] = useState("");
  const [sessionDetails, setSessionDetails] = useState(null);
  const [sessionId, setSessionId] = useState(null);
  // new Date().toISOString().split("T")[0]

  useEffect(() => {
    handleFetchSessions();
  }, []);

  const handleFetchSessions = async () => {
    const querySnapshot = await getDocs(
      query(collection(db, "LoggedData"), orderBy("date", "desc"))
    );
    const data = [];
    querySnapshot.forEach((doc) => {
      data.push({ id: doc.id, ...doc.data() });
    });
    setSessionDataFirebase(data);
  };

  const handleFetchSessionDetails = async (documentId) => {
    console.log("Fetching data for session: ", documentId);
    const docRef = doc(db, "LoggedData", documentId);
    const subCollectionSnapshot = await getDocs(
      query(collection(docRef, "Data"), orderBy("timestamp"))
    );

    const data = [];
    subCollectionSnapshot.forEach((subDoc) => {
      data.push({
        ...subDoc.data(),
        subDocId: subDoc.id,
      });
    });

    return data;
  };

  //SessionNavigation Functions
  const handleSessionChange = (date, testSequence) => {
    setSelectedDate(date);
    setSelectedTestSequence(testSequence);
    setSessionDetails(null);
    setSessionId(null);
  };

  const handleBack = () => {
    setSessionDetails(null);
    setSelectedAction(null);
    setSessionId(null);
  };

  const handleDetailsClick = async (sessionId, action) => {
    const sessionDetailsArray = await handleFetchSessionDetails(sessionId);
    setSessionDetails(sessionDetailsArray);
    setSessionId(sessionId);
    setSelectedAction(action);
  };

  return (
    <>
      <Header
        handleFetchFirebase={handleFetchSessions}
        selectedAction={selectedAction}
        sessionId={sessionId}
        handleDetailsClick={handleDetailsClick}
      />
      {sessionDataFirebase ? (
        <SessionNavigation
          sessionData={sessionDataFirebase}
          selectedDate={selectedDate}
          selectedTestSequence={selectedTestSequence}
          setSelectedTestSequence={setSelectedTestSequence}
          setSelectedDate={setSelectedDate}
          selectedAction={selectedAction}
          sessionDetails={sessionDetails}
          handleSessionChange={handleSessionChange}
          handleBack={handleBack}
          handleDetailsClick={handleDetailsClick}
        />
      ) : (
        <Loading />
      )}
      <Footer />
    </>
  );
};

export default App;
