/* eslint-disable react/prop-types */
import { Col, Image, Row } from "react-bootstrap";
import FooterImage from "../assets/footer.jpg";

const Footer = () => {
  return (
    <div className="image-handler">
      <Image
        fluid
        src={FooterImage}
        alt="Description of footer image"
        className="w-100 h-100 position-absolute object-fit-cover top-0 left-0"
      />
      <Row
        style={{ backgroundColor: "#4E008E80" }}
        className="image-handler m-0 p-0 text-light align-items-center justify-content-center"
      >
        <Col xs={6} sm={6} md={6} lg={6} className="text-center footer">
          G-28
          <br /> Redirected Walking <br />
          <h1>
            <i>Experiment</i>
          </h1>
        </Col>
        <Col xs={6} sm={6} md={6} lg={6}>
          <h1 style={{ opacity: 0.9 }}>
            <i>Teams</i>
          </h1>
          <div
            className="footer"
            style={{ fontWeight: "300", opacity: 0.7, fontSize: 12 }}
          >
            Jannaten Nayem (Project Manager)
            <br />
            Irzum Jafri (Product Owner)
            <br />
            Samu Toljamo (Software Architect)
            <br />
            Antti Santala (Software Developer)
            <br />
            Rami Nurmoranta (Software Developer)
            <br />
            Veeti Salminen (Software Developer)
            <br />
            Tommi Moilanen (Software Developer)
          </div>
          <br />
        </Col>
      </Row>
    </div>
  );
};

export default Footer;
