import { useState } from "react";
import { Box, Button } from "@chakra-ui/react";
import SessionTable from "./SessionTable";
import SessionSelector from "./SessionSelector";
import DataTable from "./DataTable";
import CoordinateGrid from "./CoordinateGrid";
// import { sessionDetails } from "../sessionData";

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
    <Box>
      {!sessionDetails ? (
        <Box>
          <SessionSelector
            onSessionChange={handleSessionChange}
            selectedDate={selectedDate}
            selectedTestSequence={selectedTestSequence}
            setSelectedTestSequence={setSelectedTestSequence}
            setSelectedDate={setSelectedDate}
          />
          <SessionTable
            sessionData={sessionData}
            selectedDate={selectedDate}
            selectedTestSequence={selectedTestSequence}
            onViewDetails={(sessionId, action) => {
              handleDetailsClick(sessionId, action);
            }}
          />
        </Box>
      ) : (
        <Box>
          <Button onClick={handleBack}>Back</Button>
          {selectedAction === "View Map" ? (
            <CoordinateGrid sessionData={sessionDetails} />
          ) : (
            <DataTable sessionData={sessionDetails} />
          )}
        </Box>
      )}
    </Box>
  );
};

export default SessionNavigation;
