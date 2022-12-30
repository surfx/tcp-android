package br.com.controltcpandroid;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.example.tcpandroid.R;

import br.com.controltcpandroid.tcpip.TcpClient;

@SuppressWarnings("CommentedOutCode")
public class MouseActivity extends AppCompatActivity {

    private TcpClient client;
    private int porta;
    private String ip;
    private TextView txtInformacoes;

    @SuppressLint("ClickableViewAccessibility")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_mouse);

        txtInformacoes = findViewById(R.id.txtInformacoes);
        Button btnClickMouse = findViewById(R.id.btnClickMouse);

        client = new TcpClient();

        getExtras();

        DisplayMetrics displayMetrics = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(displayMetrics);
        int width = displayMetrics.widthPixels;
        int height = displayMetrics.heightPixels;

        View view = findViewById(R.id.activity_mouse);
        view.setOnTouchListener((view1, event) -> onTouch(view1, event, width, height));

        btnClickMouse.setOnClickListener(v -> {
            txtInformacoes.setText("click");
            clickMouse();
        });
    }

    @SuppressWarnings("StringOperationCanBeSimplified")
    @SuppressLint("SetTextI18n")
    private void clickMouse() {
        // 4 - click mouse
        client.enviarMensagem(ip, porta, "4", mensagem -> runOnUiThread(() -> {
            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
                txtInformacoes.setText("Erro ao enviar click");
                return;
            }
            txtInformacoes.setText("mensagem: " + mensagem);
        }));
    }

    @SuppressWarnings({"SameReturnValue", "StringOperationCanBeSimplified"})
    @SuppressLint("SetTextI18n")
    private boolean onTouch(@SuppressWarnings("unused") View v, MotionEvent event, int width, int height) {
        //System.out.println("------------------------------------------");
        //System.out.println("event: " + event);
//            System.out.println(event.getX() + ", " + event.getY() + " / ("+width+", "+height+")");

        String mensagemSend = "3";
        mensagemSend += width + "x" + height + "," + (int) event.getX() + "x" + (int) event.getY();

        // 3 - send mouse pos
        client.enviarMensagem(ip, porta, mensagemSend, mensagem -> runOnUiThread(() -> {
            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
                txtInformacoes.setText("Erro ao enviar informações do mouse");
                return;
            }
            txtInformacoes.setText("mensagem: " + mensagem);
        }));

        return true;
    }

    private void getExtras() {
        Bundle extras = getIntent().getExtras();
        if (extras == null) {
            return;
        }
        ip = extras.getString("ip");
        porta = extras.getInt("porta");
    }

}