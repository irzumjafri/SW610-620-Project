/* eslint-disable react/prop-types */
// importing libraries
import { Container, Button } from "react-bootstrap";
// importing components
import DataTable from "./DataTable";
import SessionTable from "./SessionTable";
import CoordinateGrid from "./CoordinateGrid";
import SessionSelector from "./SessionSelector";

const SessionNavigation = ({
  sessionData,
  selectedDate,
  selectedTestSequence,
  selectedAction,
  sessionDetails,
  handleBack,
  handleSessionChange,
  handleDetailsClick,
  setSelectedDate,
  setSelectedTestSequence,
}) => {
  return (
    <>
      {!sessionDetails ? (
        <>
          <Container>
            <SessionSelector
              onSessionChange={handleSessionChange}
              selectedDate={selectedDate}
              selectedTestSequence={selectedTestSequence}
              setSelectedTestSequence={setSelectedTestSequence}
              setSelectedDate={setSelectedDate}
            />
          </Container>
          <SessionTable
            sessionData={sessionData}
            selectedDate={selectedDate}
            selectedTestSequence={selectedTestSequence}
            onViewDetails={(sessionId, action) => {
              handleDetailsClick(sessionId, action);
            }}
          />
        </>
      ) : (
        <div style={{ background: "#4E008E1A" }}>
          <Button className="px-5 py-5" variant="link" onClick={handleBack}>
            Back
          </Button>
          {selectedAction === "View Map" ? (
            <CoordinateGrid sessionData={sessionDetails} />
          ) : (
            <div className="px-5 pb-3">
              <DataTable sessionData={sessionDetails} />
            </div>
          )}
        </div>
      )}
    </>
  );
};

export default SessionNavigation;
