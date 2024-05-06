/* eslint-disable react/prop-types */
import React, { useContext, useEffect } from "react";
import { Card } from "react-bootstrap";
import Loading from "./Loading";
import CustomTable from "./CustomTable";
import useWindowSize from "../hooks/use-window-dimensions.hook";
import { SessionContext } from "../contexts";

const DataTable = () => {
  const { width } = useWindowSize();
  const { sessionDetails } = useContext(SessionContext);

  useEffect(() => {
    if (!sessionDetails || sessionDetails.length === 0) {
      return;
    }
  }, [sessionDetails]);

  if (!sessionDetails) {
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
        bodyData={sessionDetails.map((item, index) => (
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
                className="mb-2 w-100 rounded-0"
                style={{ border: "0.1rem solid #4E008EE6" }}
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
