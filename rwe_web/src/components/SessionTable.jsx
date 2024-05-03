/* eslint-disable react/prop-types */
import { Button, Col, Row, Table } from "react-bootstrap";
import { FileText, Map } from "react-bootstrap-icons";

function formatFirebaseTimestamp(timestamp) {
  const date = timestamp.toDate();
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Add leading zero for single-digit months
  const day = String(date.getDate()).padStart(2, "0"); // Add leading zero for single-digit days

  return `${year}-${month}-${day}`;
}

const SessionTable = ({
  sessionData,
  selectedDate,
  selectedTestSequence,
  onViewDetails,
}) => {
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
        <Table bordered hover responsive>
          <thead>
            <tr
              className="text-center"
              style={{ border: "0.1rem solid #4E008ECC" }}
            >
              <th style={{ backgroundColor: "#4E008ECC", color: "white" }}>
                Date
              </th>
              <th style={{ backgroundColor: "#4E008ECC", color: "white" }}>
                Time
              </th>
              <th style={{ backgroundColor: "#4E008ECC", color: "white" }}>
                Session ID
              </th>
              <th style={{ backgroundColor: "#4E008ECC", color: "white" }}>
                Test Sequence
              </th>
              <th style={{ backgroundColor: "#4E008ECC", color: "white" }}>
                Actions
              </th>
            </tr>
          </thead>
          <tbody>
            {filteredSessions.map((session) => (
              <tr
                key={session.id}
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
                        onClick={() => onViewDetails(session.id, "View Table")}
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
            ))}
          </tbody>
        </Table>
      )}
    </div>
  );
};

export default SessionTable;
