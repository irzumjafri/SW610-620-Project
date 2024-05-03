import { useState, useEffect } from "react";
import Loading from "./Loading";
import CoordinateGridMapper from "./CoordinateGridMapper";

const CoordinateGrid = ({ sessionData }) => {
  const [isLoading, setIsLoading] = useState(true);
  const [data, setData] = useState([]);

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
