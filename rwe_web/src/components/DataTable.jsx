import { useState, useEffect } from "react";
import { Box, Table, Thead, Tbody, Tr, Th, Td } from "@chakra-ui/react";
import Loading from "./Loading";

const DataTable = ({ sessionData }) => {
  useEffect(() => {
    if (!sessionData || sessionData.length === 0) {
      return;
    }
  }, [sessionData]);

  const tableStyles = {
    th: {
      fontWeight: "bold",
      border: "1px solid black",
    },
    td: {
      border: "1px solid black",
      textAlign: "center",
    },
  };

  if (!sessionData) {
    return <Loading />;
  } else {
    return (
      <Box overflowX="auto" margin={16}>
        <Table variant="simple" {...tableStyles.table}>
          <Thead>
            <Tr>
              <Th {...tableStyles.th}>x-coordinates</Th>
              <Th {...tableStyles.th}>real x-coordinates</Th>
              {/* <Th {...tableStyles.th}>y_coordinates</Th>
              <Th {...tableStyles.th}>real y_coordinates</Th> */}
              <Th {...tableStyles.th}>z-coordinates</Th>
              <Th {...tableStyles.th}>real z-coordinates</Th>
              <Th {...tableStyles.th}>rotation</Th>
              <Th {...tableStyles.th}>real rotation</Th>
            </Tr>
          </Thead>
          <Tbody>
            {sessionData.map((item, index) => (
              <Tr key={index}>
                <Td {...tableStyles.td}>{item["x_coordinate"]}</Td>
                <Td {...tableStyles.td}>{item["real_x_coordinate"]}</Td>
                {/* <Td {...tableStyles.td}>{item["y_coordinates"]}</Td>
                <Td {...tableStyles.td}>{item["real_y_coordinates"]}</Td> */}
                <Td {...tableStyles.td}>{item["z_coordinate"]}</Td>
                <Td {...tableStyles.td}>{item["real_z_coordinate"]}</Td>
                <Td {...tableStyles.td}>{item["rotation"]}</Td>
                <Td {...tableStyles.td}>{item["real_rotation"]}</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Box>
    );
  }
};

export default DataTable;
