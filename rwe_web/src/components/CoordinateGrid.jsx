/* eslint-disable react/prop-types */
import { useState, useEffect, useContext } from "react";
import Loading from "./Loading";
import CoordinateGridMapper from "./CoordinateGridMapper";
import { SessionContext } from "../contexts";

const CoordinateGrid = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  const { sessionDetails: sessionData } = useContext(SessionContext);

  useEffect(() => {
    setIsLoading(true);
    setData(sessionData);
    setIsLoading(false);
  }, [sessionData]);

  if (isLoading) {
    return <Loading />;
  } else {
    return <CoordinateGridMapper sessionData={data} />;
  }
};

export default CoordinateGrid;
