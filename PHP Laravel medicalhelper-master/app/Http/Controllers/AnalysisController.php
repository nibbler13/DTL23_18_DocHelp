<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;

class AnalysisController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {

        return view('analysis');
    }

    public function store(Request $request)
    {
        $params = $request->validate([
            'dateBegin' => 'required|date',
            'dateEnd' => 'required|date',
        ]);

        $response = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetDetailedInfo', $params);

        if ($response->ok()) {
            $response = $response->json();
            //dd($response);
            $data = compact('response','params');
            return view('analysis', $data);
        } else {
            return back()->withErrors(['msg' => $response->body()]);
        }
    }
}
