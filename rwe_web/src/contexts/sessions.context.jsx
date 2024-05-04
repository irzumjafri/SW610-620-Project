/* eslint-disable react/prop-types */
import { createContext, useState, useEffect } from "react";
import { collection, getDocs, doc, orderBy, query } from "firebase/firestore";
import { db } from "../../firebase";

export const SessionContext = createContext();

const SessionContextProvider = ({ children }) => {
  const [sessionDataFirebase, setSessionDataFirebase] = useState();
  const [selectedDate, setSelectedDate] = useState();
  const [selectedTestSequence, setSelectedTestSequence] = useState("");
  const [selectedAction, setSelectedAction] = useState("");
  const [sessionDetails, setSessionDetails] = useState(null);
  const [sessionId, setSessionId] = useState(null);

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
    <SessionContext.Provider
      value={{
        handleFetchFirebase: handleFetchSessions,
        selectedAction,
        sessionId,
        handleDetailsClick,
        sessionData: sessionDataFirebase,
        selectedDate,
        selectedTestSequence,
        setSelectedTestSequence,
        setSelectedDate,
        sessionDetails,
        handleSessionChange,
        handleBack,
        sessionDataFirebase,
      }}
    >
      {children}
    </SessionContext.Provider>
  );
};

export default SessionContextProvider;
