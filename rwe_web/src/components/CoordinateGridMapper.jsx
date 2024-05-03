import { useRef, useEffect } from "react";
import { Box } from "@chakra-ui/react";
import * as d3 from "d3";

const CoordinateGridMapper = ({ sessionData }) => {
  const svgRef = useRef();

  useEffect(() => {
    if (!sessionData || sessionData.length === 0) {
      return;
    }

    d3.select(svgRef.current).selectAll("*").remove();

    const margin = { top: 20, right: 30, bottom: 60, left: 60, keyMargin: 20 };
    const width = 600 - margin.left - margin.right;
    const height = 400 - margin.top - margin.bottom;

    const svg = d3
      .select(svgRef.current)
      .attr("width", width + margin.left + margin.right)
      .attr("height", height + margin.top + margin.bottom)
      .append("g")
      .attr("transform", `translate(${margin.left},${margin.top})`);

    const x = d3
      .scaleLinear()
      .domain([
        d3.min(sessionData, (d) =>
          Math.min(
            d["x_coordinate"],
            d["real_x_coordinate"],
            d["z_coordinate"],
            d["real_z_coordinate"]
          )
        ),
        d3.max(sessionData, (d) =>
          Math.max(
            d["x_coordinate"],
            d["real_x_coordinate"],
            d["z_coordinate"],
            d["real_z_coordinate"]
          )
        ),
      ])
      .nice()
      .range([0, width]);

    const y = d3
      .scaleLinear()
      .domain([
        d3.min(sessionData, (d) =>
          Math.min(
            d["z_coordinate"],
            d["real_z_coordinate"],
            d["z_coordinate"],
            d["real_z_coordinate"]
          )
        ),
        d3.max(sessionData, (d) =>
          Math.max(
            d["z_coordinate"],
            d["real_z_coordinate"],
            d["z_coordinate"],
            d["real_z_coordinate"]
          )
        ),
      ])
      .nice()
      .range([height, 0]);

    const color = "blue";
    const realColor = "red";
    const startColor = "black";

    const line = d3
      .line()
      .x((d) => x(d["x_coordinate"]))
      .y((d) => y(d["z_coordinate"]));

    svg
      .append("path")
      .datum(sessionData)
      .attr("fill", "none")
      .attr("stroke", color)
      .attr("stroke-width", 2)
      .attr("d", line);

    const realLine = d3
      .line()
      .x((d) => x(d["real_x_coordinate"]))
      .y((d) => y(d["real_z_coordinate"]));

    svg
      .append("path")
      .datum(sessionData)
      .attr("fill", "none")
      .attr("stroke", realColor)
      .attr("stroke-width", 2)
      .attr("d", realLine);

    svg
      .append("circle")
      .attr("cx", x(sessionData[sessionData.length - 1]["x_coordinate"]))
      .attr("cy", y(sessionData[sessionData.length - 1]["z_coordinate"]))
      .attr("r", 6)
      .attr("fill", color);

    svg
      .append("circle")
      .attr("cx", x(sessionData[sessionData.length - 1]["real_x_coordinate"]))
      .attr("cy", y(sessionData[sessionData.length - 1]["real_z_coordinate"]))
      .attr("r", 6)
      .attr("fill", realColor);

    svg
      .append("circle")
      .attr("cx", x(0))
      .attr("cy", y(0))
      .attr("r", 6)
      .attr("fill", startColor);
  }, [sessionData]);

  return (
    <div>
      <Box display="flex" justifyContent="center" marginBottom="10px">
        <div
          style={{ display: "flex", alignItems: "center", marginRight: "20px" }}
        >
          <svg width="20" height="20">
            <circle cx="10" cy="10" r="6" fill="blue" />
          </svg>
          <span style={{ marginLeft: "5px", color: "blue" }}>VR</span>
        </div>
        <div
          style={{ display: "flex", alignItems: "center", marginRight: "20px" }}
        >
          <svg width="20" height="20">
            <circle cx="10" cy="10" r="6" fill="red" />
          </svg>
          <span style={{ marginLeft: "5px", color: "red" }}>Real</span>
        </div>
        <div style={{ display: "flex", alignItems: "center" }}>
          <svg width="20" height="20">
            <circle cx="10" cy="10" r="6" fill="black" />
          </svg>
          <span style={{ marginLeft: "5px", color: "black" }}>Start</span>
        </div>
      </Box>
      <Box>
        <svg ref={svgRef}></svg>
      </Box>
    </div>
  );
};

export default CoordinateGridMapper;
