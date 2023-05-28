<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;

class DashboardController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {

        $codes = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetMkbCodes');

        $diseaseCodes = [];
        $doctorPosition = [];

        if ($codes->ok()) {
            $diseaseCodes = $codes->json();

        }

        $positions = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetDoctorPositions');

        if ($positions->ok()) {
            $doctorPositions = $positions->json();
        }



        return view('dashboard', compact(['diseaseCodes', 'doctorPositions']));
    }

    public function store(Request $request)
    {
        $params = $request->validate([
            'dateBegin' => 'required|date',
            'dateEnd' => 'required|date',
            'mkbCode' => '',
            'doctorPostion' => ''
        ]);


        $response = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetDashboardData', $params);


        if ($response->ok()) {
            $response = $response->json();


            $codes = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetMkbCodes');

            $diseaseCodes = [];
            $doctorPosition = [];

            if ($codes->ok()) {
                $diseaseCodes = $codes->json();

            }

            $positions = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetDoctorPositions');

            if ($positions->ok()) {
                $doctorPositions = $positions->json();
            }

            $data = compact('response','params', 'diseaseCodes', 'doctorPositions');
            return view('dashboard', $data);
        } else {
            return back()->withErrors(['msg' => $response->body()]);
        }
    }
}
