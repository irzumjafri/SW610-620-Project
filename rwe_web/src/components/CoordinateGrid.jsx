/* eslint-disable react/prop-types */
import { useState, useEffect, useContext } from "react";
import Loading from "./Loading";
import CoordinateGridMapper from "./CoordinateGridMapper";
import { SessionContext } from "../contexts";

const CoordinateGrid = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  const { sessionDetails } = useContext(SessionContext);

  useEffect(() => {
    setIsLoading(true);
    setData(sessionDetails);
    setIsLoading(false);
  }, [sessionDetails]);

  if (isLoading) {
    return <Loading />;
  } else {
    return <CoordinateGridMapper sessionData={data} />;
  }
};

export default CoordinateGrid;
