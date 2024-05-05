import { Col, Container, Row, Spinner } from "react-bootstrap";

const Loading = () => {
  return (
    <Container>
      <Row className="py-4 mb-2">
        <Col className="py-1 text-center">
          <Spinner animation="border" role="status">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </Col>
      </Row>
    </Container>
  );
};

export default Loading;
