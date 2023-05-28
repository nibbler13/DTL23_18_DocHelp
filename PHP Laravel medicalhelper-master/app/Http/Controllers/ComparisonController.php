<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;

class ComparisonController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {
        $response = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetServiceComparison');

        if ($response->ok()) {
            $response = $response->json();
            $data = compact('response');
            return view('comparison', $data);
        } else {
            return view('comparison');
        }
    }

}
