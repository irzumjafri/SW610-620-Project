import { Box, Flex, Heading, Button } from "@chakra-ui/react";

const Header = ({
  handleFetchFirebase,
  sessionId,
  handleDetailsClick,
  selectedAction,
}) => {
  const handleSyncClick = () => {
    handleFetchFirebase();
    if (sessionId) {
      handleDetailsClick(sessionId, selectedAction);
    }
  };
  return (
    <Box margin={16}>
      <Flex justify="space-between" align="center">
        <Heading as="h1" size="lg">
          Redirected Walking Experiment
        </Heading>

        <Button colorScheme="teal" variant="solid" onClick={handleSyncClick}>
          Sync Now
        </Button>
      </Flex>
    </Box>
  );
};

export default Header;
