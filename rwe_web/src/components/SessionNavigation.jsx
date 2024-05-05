/* eslint-disable react/prop-types */
// importing libraries
import { useContext } from "react";
import { Container, Button } from "react-bootstrap";
// importing components
import Loading from "./Loading";
import DataTable from "./DataTable";
import SessionTable from "./SessionTable";
import CoordinateGrid from "./CoordinateGrid";
import SessionSelector from "./SessionSelector";
import { SessionContext } from "../contexts";

const SessionNavigation = () => {
  const { selectedAction, sessionDetails, handleBack, sessionDataFirebase } =
    useContext(SessionContext);
  return (
    <>
      {sessionDataFirebase ? (
        <>
          {!sessionDetails ? (
            <>
              <Container>
                <SessionSelector />
              </Container>
              <SessionTable />
            </>
          ) : (
            <div style={{ background: "#4E008E1A" }}>
              <Button className="px-5 py-5" variant="link" onClick={handleBack}>
                Back
              </Button>
              {selectedAction === "View Map" ? (
                <CoordinateGrid />
              ) : (
                <div className="px-5 pb-3">
                  <DataTable />
                </div>
              )}
            </div>
          )}
        </>
      ) : (
        <Loading />
      )}
    </>
  );
};

export default SessionNavigation;
