/* eslint-disable react/prop-types */
import { useContext } from "react";
import { ArrowRepeat } from "react-bootstrap-icons";
import { Col, Image, Row, Button } from "react-bootstrap";
import { SessionContext } from "../contexts";
import VR_PERSON from "../assets/vr_person.jpg";

const Header = () => {
  const { handleFetchFirebase, sessionId, handleDetailsClick, selectedAction } =
    useContext(SessionContext);

  const handleSyncClick = () => {
    handleFetchFirebase();
    if (sessionId) {
      handleDetailsClick(sessionId, selectedAction);
    }
  };
  return (
    <div
      style={{
        height: "300px",
        position: "relative",
      }}
    >
      <Image
        fluid
        src={VR_PERSON}
        alt="Description of header image"
        style={{
          top: 0,
          left: 0,
          width: "100%",
          height: "100%",
          objectFit: "cover",
          position: "absolute",
        }}
      />
      <Row
        className="m-0 p-0"
        style={{
          color: "white",
          height: "300px",
          minWidth: "100%",
          position: "relative",
          alignItems: "center",
          justifyContent: "center",
          backgroundColor: "#4E008E80",
        }}
      >
        <Col xs={6} sm={6} md={6} lg={7}></Col>
        <Col xs={6} sm={6} md={6} lg={5} className="text-glow">
          <div className="mb-3">
            G-28
            <br />
            Redirected Walking <br />
            <h1>
              <i>Experiment</i>
            </h1>
          </div>
          <Button variant="" id="syncButton" onClick={handleSyncClick}>
            <ArrowRepeat id="arrayRepeat" className="me-2" />
            Sync Now
          </Button>
        </Col>
      </Row>
    </div>
  );
};

export default Header;
