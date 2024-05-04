/* eslint-disable react/prop-types */
import React, { useEffect } from "react";
import Loading from "./Loading";
import CustomTable from "./CustomTable";
import useWindowSize from "../hooks/use-window-dimentions.hook";
import { Card } from "react-bootstrap";

const DataTable = ({ sessionData }) => {
  const { width } = useWindowSize();

  useEffect(() => {
    if (!sessionData || sessionData.length === 0) {
      return;
    }
  }, [sessionData]);

  if (!sessionData) {
    return <Loading />;
  } else {
    return (
      <CustomTable
        tableHeaders={[
          "x-coordinates",
          "real x-coordinates",
          "z-coordinates",
          "real z-coordinates",
          "rotation",
          "real rotation",
        ]}
        bodyData={sessionData.map((item, index) => (
          <React.Fragment key={index}>
            {width > 875 ? (
              <tr key={index} className="text-center">
                <td>{item["x_coordinate"]}</td>
                <td>{item["real_x_coordinate"]}</td>
                <td>{item["z_coordinate"]}</td>
                <td>{item["real_z_coordinate"]}</td>
                <td>{item["rotation"]}</td>
                <td>{item["real_rotation"]}</td>
              </tr>
            ) : (
              <Card
                style={{
                  width: "100%",
                  borderRadius: "0%",
                  border: "0.1rem solid #4E008EE6",
                }}
                className="mb-2"
              >
                <Card.Body>
                  <Card.Text>
                    x-coordinates - {item["x_coordinate"]}
                    <br />
                    real x-coordinates - {item["real_x_coordinate"]}
                    <br />
                    z-coordinates - {item["z_coordinate"]}
                    <br />
                    real z-coordinates - {item["real_z_coordinate"]}
                    <br />
                    rotation - {item["rotation"]}
                    <br />
                    real-rotation - {item["real_rotation"]}
                  </Card.Text>
                </Card.Body>
              </Card>
            )}
          </React.Fragment>
        ))}
      />
    );
  }
};

export default DataTable;
