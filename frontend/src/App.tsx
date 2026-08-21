import { BrowserRouter, Routes, Route } from 'react-router'
import Home from './pages/home'
import Nav from './components/Nav'
import './App.css'

function App() {
  return (
    <BrowserRouter>
      <div>
        <div>
          <Nav />
        </div>

        <div>
          <Routes>
            <Route path="/" element={<Home />} />
          </Routes>
        </div>
      </div>
    </BrowserRouter>

  )
}

export default App
