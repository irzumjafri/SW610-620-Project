// importing components
import Header from "./components/Header";
import Footer from "./components/Footer";
import SessionNavigation from "./components/SessionNavigation";
// importing firebase
import { SessionContextProvider } from "../src/contexts";
import "./App.css";

const App = () => (
  <SessionContextProvider>
    <Header />
    <SessionNavigation />
    <Footer />
  </SessionContextProvider>
);

export default App;
