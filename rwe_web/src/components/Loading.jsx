import { Spinner, Flex } from "@chakra-ui/react";

const Loading = () => {
  return (
    <Flex
      top="0"
      left="0"
      width="100vw"
      height="100vh"
      alignItems="center"
      justifyContent="center"
    >
      <Spinner
        thickness="4px"
        speed="0.65s"
        emptyColor="gray.200"
        color="blue.500"
        size="xl"
      />
    </Flex>
  );
};

export default Loading;
