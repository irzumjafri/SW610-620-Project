/* eslint-disable react/prop-types */
import { useEffect } from "react";
import { Table } from "react-bootstrap";
import Loading from "./Loading";

const DataTable = ({ sessionData }) => {
  useEffect(() => {
    if (!sessionData || sessionData.length === 0) {
      return;
    }
  }, [sessionData]);

  if (!sessionData) {
    return <Loading />;
  } else {
    return (
      <Table bordered hover responsive>
        <thead>
          <tr className="text-center">
            <th>x-coordinates</th>
            <th>real x-coordinates</th>
            <th>z-coordinates</th>
            <th>real z-coordinates</th>
            <th>rotation</th>
            <th>real rotation</th>
          </tr>
        </thead>
        <tbody>
          {sessionData.map((item, index) => (
            <tr key={index} className="text-center">
              <td>{item["x_coordinate"]}</td>
              <td>{item["real_x_coordinate"]}</td>
              <td>{item["z_coordinate"]}</td>
              <td>{item["real_z_coordinate"]}</td>
              <td>{item["rotation"]}</td>
              <td>{item["real_rotation"]}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    );
  }
};

export default DataTable;
