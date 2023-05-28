<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;

class BatchAssignmentsController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {
        $response = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->get(env('API_URL').'/GetBatchAppointments');

        if ($response->ok()) {
            $response = $response->json();

            //группировка по разделам
            $response = array_reduce($response, function ($result, $element) {
                $key = md5($element['section']);
                $result[$key]['sectionName'] = $element['section'];
                $result[$key]['groups'][] = $element;
                return $result;
            }, []);

            $data = compact('response');
            return view('batchAssignments', $data);
        } else {
            return view('batchAssignments');
        }
    }

}
