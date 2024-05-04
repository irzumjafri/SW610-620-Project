/* eslint-disable react/prop-types */
import { Table } from "react-bootstrap";
import useWindowDimension from "../hooks/use-window-dimentions.hook";

const CustomTable = ({ tableHeaders, bodyData }) => {
  const { width } = useWindowDimension();
  return (
    <>
      {width > 875 ? (
        <Table bordered hover responsive>
          <thead>
            <tr
              className="text-center"
              style={{ border: "0.1rem solid #4E008ECC" }}
            >
              {tableHeaders.map((header) => (
                <th
                  key={header}
                  style={{ backgroundColor: "#4E008ECC", color: "white" }}
                >
                  {header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>{bodyData}</tbody>
        </Table>
      ) : (
        bodyData
      )}
    </>
  );
};

export default CustomTable;
