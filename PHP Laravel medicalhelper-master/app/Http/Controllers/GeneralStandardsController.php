<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Cache;

class GeneralStandardsController extends Controller
{
    public const SCHEDULER_CACHE_TIME = 300;
    public const CONNECTION_TIME_OUT = 180;

    public function __construct()
    {
        $this->middleware('auth');
        ini_set('max_execution_time', self::CONNECTION_TIME_OUT);
    }

    public function index()
    {
        $response = Cache::remember('generalStandards', self::SCHEDULER_CACHE_TIME, function () {
            $response = Http::connectTimeout(self::CONNECTION_TIME_OUT)->timeout(self::CONNECTION_TIME_OUT)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetGeneralStandards');
            if ($response->ok()) {
                $responseArray = $response->json();
                return $responseArray;
            }
        });
        if($response) {
            $data = compact('response');
            return view('generalStandards', $data);
        }
        else {
            return view('generalStandards');
        }


    }

}
