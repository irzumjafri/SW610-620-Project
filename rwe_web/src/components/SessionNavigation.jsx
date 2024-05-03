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
        <Container>
          <Button onClick={handleBack}>Back</Button>
          {selectedAction === "View Map" ? (
            <CoordinateGrid sessionData={sessionDetails} />
          ) : (
            <DataTable sessionData={sessionDetails} />
          )}
        </Container>
      )}
    </>
  );
};

export default SessionNavigation;
