/* eslint-disable react/prop-types */
import { useContext } from "react";
import { ArrowRepeat } from "react-bootstrap-icons";
import { Col, Image, Row, Button } from "react-bootstrap";
import { SessionContext } from "../contexts";
import VR_PERSON from "../assets/vr_person.jpg";

const Header = () => {
  const { handleFetchSessions, sessionId, handleDetailsClick, selectedAction } =
    useContext(SessionContext);

  const handleSyncClick = () => {
    handleFetchSessions();
    if (sessionId) {
      handleDetailsClick(sessionId, selectedAction);
    }
  };
  return (
    <div className="image-handler">
      <Image
        fluid
        src={VR_PERSON}
        alt="Description of header image"
        className="w-100 h-100 position-absolute object-fit-cover top-0 left-0"
      />
      <Row
        style={{ backgroundColor: "#4E008E80" }}
        className="image-handler m-0 p-0 text-light align-items-center justify-content-center"
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
