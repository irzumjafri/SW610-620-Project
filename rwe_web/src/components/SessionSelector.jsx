/* eslint-disable react/prop-types */
// SessionSelector.jsx
import { useContext } from "react";
import { Row, Form, Col } from "react-bootstrap";
import { SessionContext } from "../contexts";

const SessionSelector = () => {
  const {
    handleSessionChange: onSessionChange,
    selectedDate,
    setSelectedDate,
    selectedTestSequence,
    setSelectedTestSequence,
  } = useContext(SessionContext);

  const handleDateChange = (event) => {
    setSelectedDate(event.target.value);
    onSessionChange(event.target.value, selectedTestSequence);
  };

  const handleTestSequenceChange = (event) => {
    setSelectedTestSequence(event.target.value);
    onSessionChange(selectedDate, event.target.value);
  };

  return (
    <Row className="py-4 mb-2">
      <Col sm={12} md={12} lg={6} xl={6} className="py-1">
        <Form.Group>
          <Form.Label>Select Date</Form.Label>
          <Form.Control
            type="date"
            value={selectedDate}
            onChange={handleDateChange}
            style={{ borderRadius: "0%", border: "0.1rem solid #4E008E" }}
          />
        </Form.Group>
      </Col>
      <Col sm={12} md={12} lg={6} xl={6} className="py-1">
        <Form.Group>
          <Form.Label>Select Test Sequence</Form.Label>
          <Form.Select
            value={selectedTestSequence}
            onChange={handleTestSequenceChange}
            style={{ borderRadius: "0%", border: "0.1rem solid #4E008E" }}
          >
            <option value="">Select a test sequence</option>
            <option value="demo">Demo Sequence</option>
            <option value="sequence1">Sequence 1</option>
            <option value="sequence2">Sequence 2</option>
            <option value="sequence3">Sequence 3</option>
            <option value="sequence4">Sequence 4</option>
          </Form.Select>
        </Form.Group>
      </Col>
    </Row>
  );
};

export default SessionSelector;
