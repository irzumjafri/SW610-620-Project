/* eslint-disable react/prop-types */
import { Button, Card, Col, Row } from "react-bootstrap";
import { FileText, Map } from "react-bootstrap-icons";
import CustomTable from "./CustomTable";
import useWindowSize from "../hooks/use-window-dimensions.hook";
import React, { useContext } from "react";
import { SessionContext } from "../contexts";

function formatFirebaseTimestamp(timestamp) {
  const date = timestamp.toDate();
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Add leading zero for single-digit months
  const day = String(date.getDate()).padStart(2, "0"); // Add leading zero for single-digit days

  return `${year}-${month}-${day}`;
}

const SessionTable = () => {
  const { width } = useWindowSize();
  const {
    sessionData,
    selectedDate,
    selectedTestSequence,
    handleDetailsClick: onViewDetails,
  } = useContext(SessionContext);

  const tableHeaders = [
    "Date",
    "Time",
    "Session ID",
    "Test Sequence",
    "Actions",
  ];
  const filteredSessions = Object.values(sessionData).filter((session) => {
    if (selectedDate && formatFirebaseTimestamp(session.date) !== selectedDate)
      return false;
    if (selectedTestSequence && session.test_sequence !== selectedTestSequence)
      return false;
    return true;
  });

  return (
    <div className="px-5 py-5" style={{ background: "#4E008E1A" }}>
      {filteredSessions.length === 0 ? (
        <p>No sessions found for the selected date or test sequence.</p>
      ) : (
        <CustomTable
          tableHeaders={tableHeaders}
          bodyData={filteredSessions.map((session) => (
            <React.Fragment key={session.id}>
              {width > 875 ? (
                <tr
                  className="text-center"
                  style={{ border: "0.1rem solid #4E008ECC" }}
                >
                  <td>{session.date.toDate().toDateString()}</td>
                  <td>{session.date.toDate().toLocaleTimeString("en-US")}</td>
                  <td>{session.id}</td>
                  <td>{session.test_sequence}</td>
                  <td>
                    <Row>
                      <Col sm={12} md={12} lg={9}>
                        <Button
                          variant=""
                          className="w-100"
                          id="viewDetailsButton"
                          onClick={() =>
                            onViewDetails(session.id, "View Table")
                          }
                        >
                          <FileText className="mx-2" /> Details
                        </Button>
                      </Col>
                      <Col sm={12} md={12} lg={3}>
                        <Button
                          variant=""
                          className="m-0 p-0"
                          style={{
                            width: "100%",
                            height: "100%",
                            borderRadius: "0%",
                            border: "0.1rem solid #4E008EE6",
                          }}
                          onClick={() => onViewDetails(session.id, "View Map")}
                        >
                          <Map
                            style={{
                              color: "#4E008EE6",
                            }}
                          />
                        </Button>
                      </Col>
                    </Row>
                  </td>
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
                    <Card.Title>
                      Test Sequence - {session.test_sequence}
                    </Card.Title>
                    <Card.Text>
                      Date - {session.date.toDate().toDateString()}
                      <br />
                      Time - {session.date.toDate().toLocaleTimeString("en-US")}
                      <br />
                      Session ID -{session.id}
                    </Card.Text>
                  </Card.Body>
                  <Card.Footer>
                    <Row>
                      <Col xs={6} sm={6} md={6} lg={6}>
                        <Button
                          variant=""
                          className="w-100 flex-wrap"
                          id="viewDetailsButton"
                          onClick={() =>
                            onViewDetails(session.id, "View Table")
                          }
                        >
                          <FileText className="mx-2" /> Details
                        </Button>
                      </Col>
                      <Col xs={6} sm={6} md={6} lg={6}>
                        <Button
                          variant=""
                          className="m-0 p-0"
                          style={{
                            width: "100%",
                            height: "100%",
                            borderRadius: "0%",
                            border: "0.1rem solid #4E008EE6",
                          }}
                          onClick={() => onViewDetails(session.id, "View Map")}
                        >
                          <Map
                            style={{
                              color: "#4E008EE6",
                            }}
                          />
                        </Button>
                      </Col>
                    </Row>
                  </Card.Footer>
                </Card>
              )}
            </React.Fragment>
          ))}
        />
      )}
    </div>
  );
};

export default SessionTable;
