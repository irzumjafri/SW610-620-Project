// SessionSelector.jsx
import { Select, Box, FormControl, FormLabel, Input } from "@chakra-ui/react";
import { useState } from "react";

const SessionSelector = ({
  onSessionChange,
  selectedDate,
  setSelectedDate,
  selectedTestSequence,
  setSelectedTestSequence,
}) => {
  const handleDateChange = (event) => {
    setSelectedDate(event.target.value);
    onSessionChange(event.target.value, selectedTestSequence);
  };

  const handleTestSequenceChange = (event) => {
    setSelectedTestSequence(event.target.value);
    onSessionChange(selectedDate, event.target.value);
  };

  return (
    <Box>
      <FormControl>
        <FormLabel>Select Date</FormLabel>
        <Input type="date" value={selectedDate} onChange={handleDateChange} />
      </FormControl>
      <FormControl>
        <FormLabel>Select Test Sequence</FormLabel>
        <Select
          value={selectedTestSequence}
          onChange={handleTestSequenceChange}
          marginY={2}
        >
          <option value="">Select a test sequence</option>
          <option value="demo">Demo Sequence</option>
          <option value="sequence1">Sequence 1</option>
          <option value="sequence2">Sequence 2</option>
          <option value="sequence3">Sequence 3</option>
          <option value="sequence4">Sequence 4</option>
        </Select>
      </FormControl>
    </Box>
  );
};

export default SessionSelector;
